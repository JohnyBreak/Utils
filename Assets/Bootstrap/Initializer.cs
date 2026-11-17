using System;
using System.Collections;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;
using Bootstrap.Validation;
using NUnit.Framework;
using Unity.Android.Gradle;
using UnityEngine;

namespace Bootstrap
{
    /*
     *sync
     * async
     *sync
     *
     */
                    
                    
    /*
     *
     *
     *
     */
    
    public enum SummerStatus
    {
        None = 0,
        Success,
        Failed,
        Error
    }

    public class Initializer : IInitializer
    {
        private readonly int _retryNumber = 3;
        private readonly Dictionary<Type, int> _stages = new Dictionary<Type, int>();
        private readonly List<IExecutableModule> _sortedModules;
        private readonly IGetStageIndexStrategy _stageIndexStrategy;
        private readonly IGetOrderStrategy _orderStrategy;
        private readonly ISummerValidator _validator;
        
        public Initializer(
            Dictionary<Type, int> stages,
            List<IExecutableModule> sortedModules,
            IGetStageIndexStrategy stageIndexStrategy,
            IGetOrderStrategy orderStrategy,
            ISummerValidator validator,
            int retryNumber)
        {
            _stages = stages;
            _sortedModules = sortedModules;
            _stageIndexStrategy = stageIndexStrategy;
            _orderStrategy = orderStrategy;
            _validator = validator;
            _retryNumber = retryNumber;
        }

        public IEnumerator Start() // сделали ожидание исполнения
        {
            _validator.Validate(_sortedModules);
            
            Sort(_sortedModules);
            
            foreach (var ordered in _sortedModules)
            {
                //if (ordered.dependencies.completed())//TODO нужно сделать зависимости
                {
                    var statusHolder = new PromiseStatusHolder();
                    
                    yield return Execute(ordered, _retryNumber, statusHolder);

                    if (statusHolder.Status == SummerStatus.Error)
                    {
                        throw new SummerModuleException($"{ordered.GetType()} Module Execution Error. Initialization stopped");
                    }
                    
                    if (statusHolder.Status == SummerStatus.Failed)
                    {
                        // есть ли зарегистрированная заглушка
                        
                        // если нет заглушки, значит что-то критическое => кидаем эксепшн
                    }
                }
            }
            
            Debug.Log("complete");
        }

        private IEnumerator Execute(IExecutableModule ordered, int retryNumber, PromiseStatusHolder holder)
        {
            for (int i = 0; i < retryNumber; i++)
            {
                var promise = ordered.Execute();
                
                while (!promise.IsCompleted())
                {
                    yield return null;
                }

                var status = promise.GetStatus();

                if (status != SummerStatus.Error)
                {
                    holder.Status = promise.GetStatus();
                    yield break;
                }
            }

            holder.Status = SummerStatus.Error;
        }

        private void Sort(List<IExecutableModule> modules)
        {
            modules.Sort(
                (left, right) =>
                {
                    var stagesResult = GetStage(left).CompareTo(GetStage(right));
                    
                    if(stagesResult != 0)
                    {
                        return stagesResult;
                    }
                    
                    return GetOrder(left).CompareTo(GetOrder(right));
                });
        }

        private int GetStage(object obj)
        {
            if (!_stageIndexStrategy.TryGetStageIndex(obj, out var index))
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} нет стадии");
            }

            return index;
        }

        private int GetOrder(object obj)//TODO дублирование с валидатором, куда можно вынести ?
        {
            if (!_orderStrategy.TryGetOrder(obj, out var order))
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} нет ордера");
            }

            return order;
        }
    }
}