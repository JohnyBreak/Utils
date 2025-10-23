using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace AssetProvider
{
    public interface IUnloadStrategy
    {
        void UnloadUnused(Dictionary<IKeyEvaluator, AssetHolder> cachedAssets);
    }
    
}

