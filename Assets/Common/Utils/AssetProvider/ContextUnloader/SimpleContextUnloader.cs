using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetProvider.ContextUnloader
{
    public class SimpleContextUnloader : IContextUnloader
    {
        private readonly Dictionary<Type, Dictionary<GameObject, IKeyEvaluator>> _mapContextToKey = new();
        public bool AddToContext(Type resourceContext, GameObject gameObject, IKeyEvaluator assetId)
        {
            if (!_mapContextToKey.ContainsKey(resourceContext))
            {
                _mapContextToKey.Add(resourceContext, new Dictionary<GameObject, IKeyEvaluator>());
            }

            if (!_mapContextToKey[resourceContext].ContainsKey(gameObject))
            {
                _mapContextToKey[resourceContext].Add(gameObject, assetId);
                return true;
            }

            return false;
        }

        public void ClearAllObjectsByContext(Type resourceContext, AssetLoader assetLoader)
        {
            if (!_mapContextToKey.ContainsKey(resourceContext))
            {
                return;
            }

            var gameObjects = _mapContextToKey[resourceContext].Keys.ToList();

            foreach (var gameObject in gameObjects)
            {
                var evaluator = _mapContextToKey[resourceContext][gameObject];
                assetLoader.UnloadInstance(evaluator, gameObject);
            }
        }

        public void RemoveGameObjectFromContexts(GameObject gameObject)
        {
            foreach (var context in _mapContextToKey)
            {
                context.Value.Remove(gameObject);
            }
        }
    }
}