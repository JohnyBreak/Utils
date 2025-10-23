using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace AssetProvider
{
    public class DefaultUnloadStrategy : IUnloadStrategy
    {
        public void UnloadUnused(Dictionary<IKeyEvaluator, AssetHolder> cachedAssets)
        {
            List<IKeyEvaluator> keys = new List<IKeyEvaluator>();
            foreach (var cachedAsset in cachedAssets)
            {
                if (cachedAsset.Value.MarkToUnload)
                {
                    Addressables.Release(cachedAsset.Value.GameObject);
                    keys.Add(cachedAsset.Key);
                }
            }

            foreach (var key in keys)
            {
                cachedAssets.Remove(key);
            }
        }
    }
}