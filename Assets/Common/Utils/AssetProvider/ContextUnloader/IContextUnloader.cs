using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetProvider.ContextUnloader
{
    public interface IContextUnloader
    {
        bool AddToContext(Type resourceContext, GameObject gameObject, IKeyEvaluator assetId);
        void ClearAllObjectsByContext(Type resourceContext, AssetLoader assetLoader);
        void RemoveGameObjectFromContexts(GameObject gameObject);
    }
}