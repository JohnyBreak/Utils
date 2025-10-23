using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetProvider
{
    public class TimeUnloadStrategy : IUnloadStrategy
    {
        private readonly float _needTimeElapsed;

        public TimeUnloadStrategy(float needTimeElapsed)
        {
            _needTimeElapsed = needTimeElapsed;
        }

        public void UnloadUnused(Dictionary<IKeyEvaluator, AssetHolder> cachedAssets)
        {
            float time = Time.realtimeSinceStartup;
            List<IKeyEvaluator> keys = new List<IKeyEvaluator>();
            foreach (var cachedAsset in cachedAssets)
            {
                if (cachedAsset.Value.MarkToUnload)
                {
                    if (time - cachedAsset.Value.UsedLastTime > _needTimeElapsed)
                    {
                        Addressables.Release(cachedAsset.Value.GameObject);
                        keys.Add(cachedAsset.Key);
                    }
                }
            }

            foreach (var key in keys)
            {
                cachedAssets.Remove(key);
            }
        }
    }
}