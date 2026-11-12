using System;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;

namespace Bootstrap.Validation
{
    public class SummerValidator : ISummerValidator
    {
        private readonly IGetOrderStrategy _orderStrategy;

        public SummerValidator(IGetOrderStrategy orderStrategy)
        {
            _orderStrategy = orderStrategy;
        }

        public void Validate(List<IExecutableModule> modules)
        {
            ValidateDuplicatedModules(modules);
            ValidateDuplicatedOrders(modules);
            ValidateModules(modules);
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
        
        private int GetOrder(object obj)//TODO дублирование с инициализатором, куда можно вынести ?
        {
            if (!_orderStrategy.TryGetOrder(obj, out var order))
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} нет ордера");
            }

            return order;
        }
        
        private void ValidateModules(List<IExecutableModule> sortedModules)
        {
            foreach (var module in sortedModules)
            {
                if (SummerHelper.HasSummerAttributes<SummerSettingsAttribute>(module) && module is ISummerModule)
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
                if (SummerHelper.HasSummerAttributes<SummerDuplicateAttribute>(module))
                {
                    continue;
                }

                if (!modulesTypes.Add(module.GetType()))
                {
                    throw new SummerModuleException($"Дубль Модуля с типом {module.GetType()}");
                }
            }
        }
    }
}