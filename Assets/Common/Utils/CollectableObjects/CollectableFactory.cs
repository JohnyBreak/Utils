using System;
using System.Collections.Generic;
using Collectables.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Collectables
{
    public class CollectableFactory
    {
        private readonly AssetProvider _assetProvider;
        private readonly Action<CollectableObjectView> _onEnter;
        private readonly Action<CollectableObjectView> _onExit;
        private readonly Action<CollectableObjectView, Action, Action> _onCollect;

        private readonly Dictionary<int, string> _keysMap = new()
        {
            {CollectablesTypes.PistolAmmo, "PistolAmmo"}
        };

        public CollectableFactory(
            AssetProvider assetProvider,
            Action<CollectableObjectView> onEnter,
            Action<CollectableObjectView> onExit,
            Action<CollectableObjectView, Action, Action> onCollect)
        {
            _assetProvider = assetProvider;
            _onEnter = onEnter;
            _onExit = onExit;
            _onCollect = onCollect;
        }

        public async UniTask SpawnObjectAt(CollectableConfig config, Vector3 position)
        {
            var prefab = await GetPrefab(config.GetCollectableType());
            
            if (!prefab)
            {
                throw new Exception("prefab in config is null");
            }

            var loot = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity)
                .GetComponent<CollectableObjectView>();
            loot.Init(config, _onEnter, _onExit, _onCollect);
        }

        private async UniTask<GameObject> GetPrefab(int type)
        {
            if (!_keysMap.TryGetValue(type, out string key))
            {
                throw new Exception($"_keysMap is not contains key for CollectablesTypes.{key} type");
            }

            return await _assetProvider.LoadAssetAsync<GameObject>(key);
        }
    }
}