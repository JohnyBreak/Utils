using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetProvider.ContextUnloader
{
    public class EmptyContextUnloader: IContextUnloader
    {
        public bool AddToContext(Type resourceContext, GameObject gameObject, IKeyEvaluator assetId)
        {
            return true;
        }

        public void ClearAllObjectsByContext(Type resourceContext, AssetLoader assetLoader)
        {
        }

        public void RemoveGameObjectFromContexts(GameObject gameObject)
        {
            Debug.LogError("[ResourceManager/ContextUnloader] this is empty! So, I do nothing...");
        }
    }
}