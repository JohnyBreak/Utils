using System;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;
using Bootstrap.Validation;
using UnityEngine;

namespace Bootstrap
{
    public class Initializer : IInitializer
    {
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
            ISummerValidator validator)
        {
            _stages = stages;
            _sortedModules = sortedModules;
            _stageIndexStrategy = stageIndexStrategy;
            _orderStrategy = orderStrategy;
            _validator = validator;
        }

        public void Start()
        {
            _validator.Validate(_sortedModules);
            
            Sort(_sortedModules);//TODO вынести в отдельную стратегию или вообще выполнить снаружи, а сюда передать готовый список модулей ?
            
            foreach (var ordered in _sortedModules)
            {
                ordered.Execute();
            }
            
            Debug.Log("complete");
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