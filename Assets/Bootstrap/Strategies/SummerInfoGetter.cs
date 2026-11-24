using System;
using System.Collections.Generic;

namespace Bootstrap.Strategies
{
    public class SummerInfoGetter : ISummerInfoGetter, IGetStageStrategy, IGetOrderStrategy, IGetDependenciesStrategy
    {
        private readonly List<IGetStageStrategy> _stageStrategies;
        private readonly List<IGetOrderStrategy> _orderStrategies;
        private readonly List<IGetDependenciesStrategy> _dependenciesStrategies;
        
        public SummerInfoGetter(
            List<IGetStageStrategy> stageStrategies,
            List<IGetOrderStrategy> orderStrategies,
            List<IGetDependenciesStrategy> dependenciesStrategies)
        {
            _stageStrategies = stageStrategies;
            _orderStrategies = orderStrategies;
            _dependenciesStrategies = dependenciesStrategies;
        }

        public bool TryGetStageIndex(object obj, out int index)
        {
            foreach (var strategy in _stageStrategies)
            {
                if (strategy.TryGetStageIndex(obj, out var innerIndex))
                {
                    index = innerIndex;
                    return true;
                }
            }
            
            index = 0;
            return true;
        }

        public bool TryGetStageType(object obj, out Type stageType)
        {
            foreach (var strategy in _stageStrategies)
            {
                if (strategy.TryGetStageType(obj, out var innerType))
                {
                    stageType = innerType;
                    return true;
                }
            }

            stageType = null;
            return false;
        }

        public bool TryGetOrder(object obj, out int order)
        {
            foreach (var strategy in _orderStrategies)
            {
                if (strategy.TryGetOrder(obj, out var innerOrder))
                {
                    order = innerOrder;
                    return true;
                }
            }
            
            order = 0;
            return true;
        }

        public bool TryGetDependencies(object obj, out Type[] dependencies)
        {
            foreach (var strategy in _dependenciesStrategies)
            {
                if (strategy.TryGetDependencies(obj, out var innerValue))
                {
                    dependencies = innerValue;
                    return true;
                }
            }

            dependencies = null;
            return false;
        }
    }
}