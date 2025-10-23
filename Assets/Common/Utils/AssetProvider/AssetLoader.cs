using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetProvider.Context;
using AssetProvider.ContextUnloader;
using AssetProvider.DestroyStrategy;
using AssetProvider.LoadingQueue;
using Common.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace AssetProvider
{
    public class AssetLoader
    {
        private readonly Dictionary<IKeyEvaluator, AssetHolder> _cachedAssets = new();
        private readonly Dictionary<IKeyEvaluator, HashSet<GameObject>> _instantiatedObjects = new();
        private readonly AddressableLoadingQueue _addressableLoadingQueue = new AddressableLoadingQueue();
        private List<IDownloadHandler> _handlers = new List<IDownloadHandler>();
        
        private readonly IDestroyStrategy _destroyStrategy;
        private readonly IUnloadStrategy _unloadStrategy;
        private readonly IContextUnloader _contextUnloader;

        public AssetLoader(IDestroyStrategy destroyStrategy, IUnloadStrategy unloadStrategy)
        {
            _destroyStrategy = destroyStrategy;
            _unloadStrategy = unloadStrategy;
            _contextUnloader = new EmptyContextUnloader();
        }

        public AssetLoader(IDestroyStrategy destroyStrategy, IUnloadStrategy unloadStrategy, IContextUnloader contextUnloader)
        {
            _destroyStrategy = destroyStrategy;
            _unloadStrategy = unloadStrategy;
            _contextUnloader = contextUnloader;
        }
        
        public bool IsAssetLoaded<T>(IKeyEvaluator assetId) where T : UnityEngine.Object
        {
            var result = GetGameObjectFromCache<T>(assetId);
            return result.IsExist;
        }

        public Result<T> LoadSync<T>(IKeyEvaluator assetId) where T : UnityEngine.Object
        {
            var cacheResult = GetGameObjectFromCache<T>(assetId);
            if (cacheResult.IsExist)
            {
                return cacheResult;
            }
            
            T result = Addressables.LoadAssetAsync<T>(assetId).WaitForCompletion();
            
            if (result == default)
            {
                return new Result<T>(result, false);
            }
            
            ConfigureAssetHolderAndCacheAfterLoad(assetId, result);
            return new Result<T>(result, true);
        }
        
        public async UniTask<T> LoadAsync<T>(IKeyEvaluator assetId) where T : UnityEngine.Object
        {
            var cacheResult = GetGameObjectFromCache<T>(assetId);
            if (cacheResult.IsExist)
            {
                return cacheResult.Object;
            }

            T result;
            bool isContains = false;
            if (!_addressableLoadingQueue.Contains(assetId))
            {
                var handle = new DownloadHandler<T>(assetId);
                _addressableLoadingQueue.Enqueue(new LoadingInfo(assetId, handle));
                await UniTask.WaitUntil(() => handle.IsInited());
                result = await handle.GetTask();
            }
            else
            {
                var downloadHandler = _addressableLoadingQueue.GetHandle(assetId) as DownloadHandler<T>;
                if (downloadHandler == null)
                {
                    Debug.LogError("DownloadHandler is null, u're doing something wrong");
                    return default;
                }
                
                await UniTask.WaitUntil(() => downloadHandler.IsInited());
                result = await downloadHandler.GetTask();
                isContains = true;
            }

            if (!isContains)
            {
                ConfigureAssetHolderAndCacheAfterLoad(assetId, result);
            }

            return result;
        }

        public IEnumerator Load<T>(IKeyEvaluator assetId, Action<T> onComplete) where T : UnityEngine.Object 
        {
            var cacheResult = GetGameObjectFromCache<T>(assetId);
            if (cacheResult.IsExist)
            {
                onComplete.Invoke(cacheResult.Object);
                yield break;
            }

            bool isContains = false;
            DownloadHandler<T> handle;
            if (!_addressableLoadingQueue.Contains(assetId))
            {
                handle = new DownloadHandler<T>(assetId);
                _addressableLoadingQueue.Enqueue(new LoadingInfo(assetId, handle));
            }
            else
            {
                isContains = true;
                handle = _addressableLoadingQueue.GetHandle(assetId) as DownloadHandler<T>;
            }
            
            yield return handle.WaitDone();
            T result = handle.Result;
            if (!isContains)
            {
                var assetHolder = ConfigureAssetHolderAndCacheAfterLoad(assetId, result);
                onComplete.Invoke(assetHolder.GameObject as T);
            }
            else
            {
                onComplete.Invoke(result);
            }
        }
        
        public void RequestToLoadAddressableAsset<T>(IKeyEvaluator assetId) where T : UnityEngine.Object
        {
            if (IsAssetLoaded<T>(assetId))
            {
                return;
            }

            if (!_addressableLoadingQueue.Contains(assetId))
            {
                var handle = new DownloadHandler<T>(assetId);
                _addressableLoadingQueue.Enqueue(new LoadingInfo(assetId, handle));
            }
        }
        
        public void ClearAllObjectsByContext(Type resourceContext)
        {
            _contextUnloader.ClearAllObjectsByContext(resourceContext, this);
        }

        public IEnumerator Instantiate<T>(
            IKeyEvaluator assetId,
            Action<T> onComplete,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = default,
            Type resourceContext = null)
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            GameObject result = null;
            yield return Load<GameObject>(assetId, (a) => { result = a; });

            var obj = CreateObjectClone(assetId, result, resourceContext, position, rotation, parent);

            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId} no component {typeof(T)} was found");
                yield break;
            }
            
            onComplete.Invoke(component);
        }

        public async UniTask<T> InstantiateAsync<T>(
            IKeyEvaluator assetId,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = default,
            Type resourceContext = null) 
            
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            var result = await LoadAsync<GameObject>(assetId);
            var obj = CreateObjectClone(assetId, result, resourceContext, position, rotation, parent);
            
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId}");
                return null;
            }
            
            return component;
        }
        
        public T InstantiateSync<T>(
            IKeyEvaluator assetId,
            Vector3 position,
            Quaternion rotation,
            Transform parent = default,
            Type resourceContext = null) 
            
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            var result = LoadSync<GameObject>(assetId);
            if (!result.IsExist)
            {
                Debug.LogError($"[ResourceManager] asset with id: {assetId} is not loaded. And cannot be loaded.");
                return null;
            }
            
            var obj = CreateObjectClone(assetId, result.Object, resourceContext, position, rotation, parent);
            
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId}");
                return null;
            }
            
            return component;
        }
        
        public T InstantiateSync<T>(
            IKeyEvaluator assetId,
            Transform parent = default,
            Type resourceContext = null) 
            
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            var result = LoadSync<GameObject>(assetId);
            if (!result.IsExist)
            {
                Debug.LogError($"[ResourceManager] asset with id: {assetId} is not loaded. And cannot be loaded.");
                return null;
            }
            
            var obj = CreateObjectClone(assetId, result.Object, resourceContext, parent);
            
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId}");
                return null;
            }
            
            return component;
        }
        
        public T InstantiateSync<T>(
            IKeyEvaluator assetId,
            TransformParams transformParams = default,
            Type resourceContext = null) 
            
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            var result = LoadSync<GameObject>(assetId);
            if (!result.IsExist)
            {
                Debug.LogError($"[ResourceManager] asset with id: {assetId} is not loaded. And cannot be loaded.");
                return null;
            }
            
            var obj = CreateObjectClone(assetId, result.Object, 
                resourceContext, 
                transformParams.Position, 
                transformParams.Rotation, 
                transformParams.Parent);
            
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId}");
                return null;
            }
            
            return component;
        }
        
        public IEnumerator Instantiate<T>(
            IKeyEvaluator assetId,
            Action<T> onComplete,
            TransformParams transformParams = default,
            Type resourceContext = null)
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            GameObject result = null;
            yield return Load<GameObject>(assetId, (a) => { result = a; });

            var obj = CreateObjectClone(assetId, result, 
                resourceContext, 
                transformParams.Position, 
                transformParams.Rotation, 
                transformParams.Parent);

            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId} no component {typeof(T)} was found");
                yield break;
            }
            
            onComplete.Invoke(component);
        }

        public async UniTask<T> InstantiateAsync<T>(
            IKeyEvaluator assetId,
            TransformParams transformParams = default,
            Type resourceContext = null) 
            
            where T : Component
        {
            if (resourceContext == null)
            {
                resourceContext = typeof(SceneResourceContext);
            }
            
            var result = await LoadAsync<GameObject>(assetId);
            var obj = CreateObjectClone(assetId, result, 
                resourceContext, 
                transformParams.Position, 
                transformParams.Rotation, 
                transformParams.Parent);
            
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"[ResourceManager] error with load asset with id: {assetId}");
                return null;
            }
            
            return component;
        }

        public void UnloadInstance(IKeyEvaluator assetId, Component component)
        {
            UnloadInstance(assetId, component.gameObject);
        }
        
        public void UnloadInstance(IKeyEvaluator assetId, GameObject gameObject)
        {
            if (_instantiatedObjects.ContainsKey(assetId))
            {
                if (gameObject == null)
                {
                    Debug.LogError("[ResourceManager] I cant let you unload objects without reference. "
                    + "If u want unload resource, u must call UnloadResourceImmediate(assetId)");
                    return;
                }

                var wasRemoved = _instantiatedObjects[assetId].Remove(gameObject);
                if (!wasRemoved)
                {
                    Debug.LogError("[ResourceManager] This object cannot be unload cause it is not exists in cache");
                    return;
                }

                _contextUnloader.RemoveGameObjectFromContexts(gameObject);
                
                _destroyStrategy.Destroy(gameObject);
                if (_instantiatedObjects[assetId].Count > 0)
                {
                    return;
                }
                
                if (_instantiatedObjects[assetId].Count == 0)
                {
                    if (!_cachedAssets.ContainsKey(assetId))
                    {
                        Debug.LogError("[ResourceManager] consistency error: instantiate objects exists but cached is not");
                        return;
                    }
                }
            }

            UnloadAsset(assetId);
        }

        public void UnloadAsset(IKeyEvaluator assetId)
        {
            if (!_cachedAssets.TryGetValue(assetId, out var assetHolder))
            {
                Debug.LogError($"asset with key {assetId.RuntimeKey} is not cached. Check names");
                return;
            }
            
            _cachedAssets[assetId].MarkToUnload = true;
            _cachedAssets[assetId].UsedLastTime = Time.realtimeSinceStartup;
        }

        public IEnumerator NeedToPreload(IKeyEvaluator assetId, Action<IKeyEvaluator, bool> onCompleteOperation)
        {
            var downloadSizeOperation = Addressables.GetDownloadSizeAsync(assetId);
            while (downloadSizeOperation.Status == AsyncOperationStatus.None)
            {
                yield return null;
            }
            onCompleteOperation.Invoke(
                assetId,
                downloadSizeOperation.Status == AsyncOperationStatus.Succeeded
                && downloadSizeOperation.Result != 0
            );
            Addressables.Release(downloadSizeOperation); 
        }
        
        public IEnumerator Preload(IKeyEvaluator assetId, Action<bool, IKeyEvaluator> onComplete = null, Action<float> onProgress = null) 
        {
            bool needToPreload = true;
            yield return NeedToPreload(assetId, (id, need) =>
            {
                needToPreload = need;
            });

            if (!needToPreload)
            {
                onComplete.Invoke(true, assetId);
                yield break;
            }
            
            var downloadHandle = Addressables.DownloadDependenciesAsync(assetId, false);
            float progress = 0;
            while (downloadHandle.Status == AsyncOperationStatus.None) 
            {
                float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
                if (percentageComplete > progress * 1.1) 
                {
                    progress = percentageComplete; 
                    onProgress?.Invoke(progress);
                }
                yield return null;
            }

            onComplete.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded, assetId);
            Addressables.Release(downloadHandle); 
        }
        
        public async Task<bool> NeedToPreloadAsync(IKeyEvaluator assetId)
        {
            var downloadSizeOperation = Addressables.GetDownloadSizeAsync(assetId);
            await downloadSizeOperation.Task;
            
            var needToPreload = downloadSizeOperation.Status == AsyncOperationStatus.Succeeded
                                && downloadSizeOperation.Result != 0;
            Addressables.Release(downloadSizeOperation);
            return needToPreload;
        }
        
        public async Task<bool> PreloadAsync(IKeyEvaluator assetId, Action<float> onProgress = null) 
        {
            var needToPreload = await NeedToPreloadAsync(assetId);

            if (!needToPreload)
            {
                return true;
            }

            float progress = 0;

            onProgress?.Invoke(0);
            var downloadHandle = Addressables.DownloadDependenciesAsync(assetId);
            while (!downloadHandle.IsDone)
            {
                onProgress?.Invoke(downloadHandle.PercentComplete);
                await Task.Yield();
            }

            var downloadHandleStatus = downloadHandle.Status;
            Addressables.Release(downloadHandle);

            return downloadHandleStatus == AsyncOperationStatus.Succeeded;
        }

        public void UnloadUnusedAssets()
        {
            _unloadStrategy.UnloadUnused(_cachedAssets);
        }

        public bool IsDownloadingQueueNotEmpty()
        {
            return _addressableLoadingQueue.GetCount() > 0;
        }

        public IEnumerator DownloadAll()
        {
            while (true)
            {
                if (IsDownloadingQueueNotEmpty())
                {
                    for (int i = 0; i < _addressableLoadingQueue.GetCount(); ++i)
                    {
                        var loadingInfo = _addressableLoadingQueue.GetByIndex(i);
                        var assetId = loadingInfo.Object.Key;
                        var handle = loadingInfo.Object.Handler;
                        loadingInfo.Object.Handler.Handle(this);
                        _handlers.Add(loadingInfo.Object.Handler);
                       
                    }

                    for (int i = 0; i < _handlers.Count; ++i)
                    {
                        yield return new WaitUntil(_handlers[i].IsInProgress);
                    }
                    _handlers.Clear();
                }
                
                yield return null;
            }
            
        }
        
        private Result<T> GetGameObjectFromCache<T>(IKeyEvaluator assetId) where T : UnityEngine.Object
        {
            if (_cachedAssets.TryGetValue(assetId, out var assetHolder))
            {
                assetHolder.UsedLastTime = Time.realtimeSinceStartup;
                assetHolder.MarkToUnload = false;
                return new Result<T>(assetHolder.GameObject as T, true);
            }
            
            return new Result<T>(default, false);
        }

        internal AssetHolder ConfigureAssetHolderAndCacheAfterLoad<T>(IKeyEvaluator assetId, T result) where T : UnityEngine.Object
        {
            var assetHolder = new AssetHolder(result, Time.realtimeSinceStartup, assetId, true);
            _cachedAssets.Add(assetId, assetHolder);
            _addressableLoadingQueue.Remove(assetId);
            return assetHolder;
        }

        private GameObject CreateObjectClone(
            IKeyEvaluator assetId,
            GameObject result,
            Type resourceContext,
            Vector3 position,
            Quaternion rotation,
            Transform parent = default)
        {
            if (result == null)
            {
                Debug.LogError("I think you want instantiate Sprite/TextAsset object or something like that");
            }
            
            GameObject obj = Object.Instantiate(result, position, rotation, parent);
            SetOptions(assetId, resourceContext, obj);

            return obj;
        }
        
        private GameObject CreateObjectClone(
            IKeyEvaluator assetId,
            GameObject result,
            Type resourceContext,
            Transform parent = default)
        {
            if (result == null)
            {
                Debug.LogError("I think you want instantiate Sprite/TextAsset object or something like that");
            }
            
            GameObject obj = Object.Instantiate(result, parent);
            SetOptions(assetId, resourceContext, obj);

            return obj;
        }
        
        private void SetOptions(IKeyEvaluator assetId, Type resourceContext, GameObject obj)
        {
            if (!_instantiatedObjects.ContainsKey(assetId))
            {
                _instantiatedObjects.Add(assetId, new HashSet<GameObject>());
            }

            var isAddedInCache = _instantiatedObjects[assetId].Add(obj);
            if (!isAddedInCache)
            {
                Debug.LogError("[ResourceManager] this is strange, new instantiate object already exist in cache");
            }
            else
            {
                _cachedAssets[assetId].MarkToUnload = false;
            }

            if (!_contextUnloader.AddToContext(resourceContext, obj, assetId))
            {
                Debug.LogError("[ResourceManager] this is strange, new instantiate object already exist in context map");
            }
        }
        
        private void UnloadResourceImmediate(IKeyEvaluator assetId)
        {
            if (_cachedAssets.ContainsKey(assetId))
            {
                var handle = _cachedAssets[assetId].GameObject;
                Addressables.Release(handle);
                _cachedAssets.Remove(assetId);
            }
        }
    }
}