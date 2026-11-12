using System.Collections.Generic;

namespace Bootstrap.Strategies
{
    public class SummerInfoGetter : IGetStageIndexStrategy, IGetOrderStrategy
    {
        private readonly List<IGetStageIndexStrategy> _stageStrategies;
        private readonly List<IGetOrderStrategy> _orderStrategies;
        
        public SummerInfoGetter(
            List<IGetStageIndexStrategy> stageStrategies,
            List<IGetOrderStrategy> orderStrategies)
        {
            _stageStrategies = stageStrategies;
            _orderStrategies = orderStrategies;
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
    }
}