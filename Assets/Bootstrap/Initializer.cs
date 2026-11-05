using System;
using System.Collections.Generic;
using Bootstrap.Editor;
using UnityEngine;

namespace Bootstrap
{
    public class Initializer : IInitializer
    {
        private readonly List<IExecutableModule> _sortedModules;

        public Initializer(List<IExecutableModule> sortedModules)
        {
            _sortedModules = sortedModules;
        }

        public void Start()
        {
            // не дублировать сами модули
            ValidateDuplicatedModules(_sortedModules);
            ValidateDuplicatedOrders(_sortedModules);
            
            ValidateModules(_sortedModules);
            
            Sort(_sortedModules);
            
            foreach (var ordered in _sortedModules)
            {
                ordered.Execute();
            }
            
            Debug.Log("complete");
        }

        private void Sort(List<IExecutableModule> ordereds)
        {
            ordereds.Sort((left, right) => GetOrder(left).CompareTo(GetOrder(right)));
        }

        private int GetOrder(object obj)
        {
            if (obj is IOrdered ordered)
            {
                return ordered.GetOrder();
            }
            
            var attributes = GetSummerAttributes(obj);

            if (attributes.Count < 1)
            {
                return int.MaxValue;
            }

            return attributes[0].Order;
        }

        private List<SummerSettingsAttribute> GetSummerAttributes(object obj)
        {
            List<SummerSettingsAttribute> result = new List<SummerSettingsAttribute>();
            
            var customAttributes = obj.GetType().GetCustomAttributes(typeof(SummerSettingsAttribute), false);
                
            foreach (var customAttribute in customAttributes)
            {
                if(customAttribute is SummerSettingsAttribute summerSettings)
                {
                    result.Add(summerSettings);
                }
            }

            return result;
        }

        private void ValidateDuplicatedOrders(List<IExecutableModule> sortedModules)
        {
            Dictionary<int, Type> ordersMap = new Dictionary<int, Type>(sortedModules.Count);

            foreach (var module in sortedModules)// можно разрешить дубли int.MaxValue
            {
                var order = GetOrder(module);
                
                if(!ordersMap.TryAdd(order, module.GetType()))
                {
                    throw new SummerModuleException($"Модули типов {ordersMap[order]} и {module.GetType()} имеют одинаковый order {order}. Дублей не должно быть");
                }
            }
        }

        private void ValidateModules(List<IExecutableModule> sortedModules)
        {
            foreach (var module in sortedModules)
            {
                if (HasSummerAttributes<SummerSettingsAttribute>(module) && module is IOrdered)
                {
                    throw new SummerModuleException("Module has SummerSettingsAttribute and has implemented IOrdered.");//TODO переписать сообщение
                }
            }
        }
        
        private void ValidateDuplicatedModules(List<IExecutableModule> sortedModules)
        {
            HashSet<Type> modulesTypes = new HashSet<Type>(sortedModules.Count);
            
            foreach (var module in sortedModules)
            {
                if (HasSummerAttributes<SummerDuplicateAttribute>(module))
                {
                    continue;
                }

                if (!modulesTypes.Add(module.GetType()))
                {
                    throw new SummerModuleException($"Дубль Модуля с типом {module.GetType()}");
                }
            }
        }
        
        private bool HasSummerAttributes<T>(object obj) where T : Attribute
        {
            var result = obj.GetType().GetCustomAttributes(typeof(T), false);
            return result.Length > 0;
        }
    }

    public class SummerModuleException : Exception
    {
        public SummerModuleException(string message) : base(message)
        {
            
        }
    }
}