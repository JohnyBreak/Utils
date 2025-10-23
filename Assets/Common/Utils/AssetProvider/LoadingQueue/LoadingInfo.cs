using UnityEngine.AddressableAssets;

namespace AssetProvider.LoadingQueue
{
    public class LoadingInfo
    {
        private IKeyEvaluator _key;
        private IDownloadHandler _handler;
        public IKeyEvaluator Key => _key;
        public IDownloadHandler Handler => _handler;
        
        public LoadingInfo(IKeyEvaluator key, IDownloadHandler handler)
        {
            _key = key;
            _handler = handler;
        }
    }
}