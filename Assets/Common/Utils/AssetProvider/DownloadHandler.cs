using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetProvider
{
    public class DownloadHandler<T> : IDownloadHandler where T : UnityEngine.Object
    {
        private AsyncOperationHandle<T> _handle;
        private bool _isInited;
        private IKeyEvaluator _key;
        private bool _isDone;

        public DownloadHandler(IKeyEvaluator key)
        {
            _key = key;
        }

        public T Result => _handle.Result;

        public UniTask<T> GetTask()
        {
            return _handle.Task.AsUniTask();
        }

        public IEnumerator WaitDone()
        {
            while (!_isInited)
            {
                yield return true;
            }
                
            yield return _handle;
        }

        public bool IsInited()
        {
            return _isInited;
        }
        
        public async UniTaskVoid Handle(AssetLoader assetLoader)
        {
            _handle = Addressables.LoadAssetAsync<T>(_key);
            _isInited = true;
            T result = await _handle;
            assetLoader.ConfigureAssetHolderAndCacheAfterLoad(_key, result);
            _isDone = true;
        }

        public bool IsInProgress()
        {
            return _isDone;
        }
    }
}

