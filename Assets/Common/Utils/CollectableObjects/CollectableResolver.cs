using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Collectables
{
    public class CollectableResolver
    {
        private readonly Dictionary<int, ICollector> _collectorsMap;
        
        public CollectableResolver(List<ICollector> collectors)
        {
            _collectorsMap = collectors.ToDictionary(x => x.GetCollectableType());
        }
        
        public bool TryCollect(CollectableConfig collectableConfig)
        {
            if (collectableConfig == null)
            {
                Debug.LogError($"Collectable is null");
                return false;
            }

            if (_collectorsMap.TryGetValue(collectableConfig.GetCollectableType(), out var collector))
            {
                return collector.TryCollect(collectableConfig);
            }
            
            Debug.LogError($"No collector for '{collectableConfig.GetCollectableType()}' CollectableType");
            return false;
        }
    }
}