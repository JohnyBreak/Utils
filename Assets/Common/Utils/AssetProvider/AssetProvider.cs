using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

public sealed class AssetProvider : IDisposable
{
    private readonly HashSet<AsyncOperationHandle> _activeHandles = new();
    private readonly Dictionary<string, AsyncOperationHandle> _loadedHandles = new();
    private readonly List<GameObject> _spawnedInstances = new();
    private readonly Dictionary<string, int> _refCounts = new(); 

    public async UniTask<AsyncOperationHandle<T>> LoadAssetAsyncWithHandle<T>(string key) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        _activeHandles.Add(handle);
        _loadedHandles[key] = handle;
        await handle.ToUniTask();
        return handle;
    }

    public async UniTask<AsyncOperationHandle<T>> LoadAssetAsyncWithHandle<T>(AssetReference reference) where T : Object
    {
        return await LoadAssetAsyncWithHandle<T>(reference.AssetGUID);
    }
    
    public async UniTask<T> LoadAssetAsync<T>(string key) where T : Object
    {
        if (IncrementRefCount(key))
        {
            var cachedHandle = _loadedHandles[key];
            return cachedHandle.Result as T;
        }
        
        var handle = Addressables.LoadAssetAsync<T>(key);
        _activeHandles.Add(handle);
        _loadedHandles[key] = handle;
        
        await handle.ToUniTask();
        return handle.Result;
    }
    
    public async UniTask<T> LoadAssetAsync<T>(AssetReference reference) where T : Object
    {
        return await LoadAssetAsync<T>(reference.AssetGUID);
    }
    
    public void Release<T>(AsyncOperationHandle<T> handle)
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
            _activeHandles.Remove(handle);
        }
    }

    public void Release(string key)
    {
        if (!_loadedHandles.TryGetValue(key, out var handle))
        {
            return;
        }
        
        if (!handle.IsValid())
        {
            _loadedHandles.Remove(key);
            _refCounts.Remove(key);
            return;
        }

        _refCounts[key]--;
        if (_refCounts[key] > 0)
        {
            return;
        }
        
        Addressables.Release(handle);
        _activeHandles.Remove(handle);
        _loadedHandles.Remove(key);
        _refCounts.Remove(key);
    }
    
    public T LoadAssetSync<T>(string key) where T : Object
    {
        if (IncrementRefCount(key))
        {
            var cachedHandle = _loadedHandles[key];
            return cachedHandle.Result as T;
        }
        
        var handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion();

        _activeHandles.Add(handle);
        _loadedHandles[key] = handle;

        return handle.Result;
    }

    public T LoadAssetSync<T>(AssetReference reference) where T : Object
    {
        return LoadAssetSync<T>(reference.AssetGUID);
    }

    public async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null)
    {
        var handle = Addressables.InstantiateAsync(key, parent);
        _activeHandles.Add(handle);
        var instance = await handle.ToUniTask();

        _spawnedInstances.Add(instance);
        return instance;
    }

    public async UniTask<GameObject> InstantiateAsync(AssetReference reference, Transform parent = null)
    {
        return await InstantiateAsync(reference.AssetGUID, parent);
    }

    public GameObject InstantiateSync(string key, Transform parent = null)
    {
        var handle = Addressables.InstantiateAsync(key, parent);
        handle.WaitForCompletion();

        _activeHandles.Add(handle);
        var instance = handle.Result;
        _spawnedInstances.Add(instance);

        return instance;
    }

    public GameObject InstantiateSync(AssetReference reference, Transform parent = null)
    {
        return InstantiateSync(reference.AssetGUID, parent);
    }

    public void ReleaseInstance(GameObject instance)
    {
        if (instance == null) return;

        Addressables.ReleaseInstance(instance);
        _spawnedInstances.Remove(instance);
    }

    public void CleanUp()
    {
        foreach (var instance in _spawnedInstances)
        {
            if (instance != null)
                Addressables.ReleaseInstance(instance);
        }
        _spawnedInstances.Clear();

        foreach (var handle in _activeHandles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        _activeHandles.Clear();
        _loadedHandles.Clear();

        // (опционально) Можно принудительно освободить память:
        // Resources.UnloadUnusedAssets().Forget();
        // System.GC.Collect();
    }
    
    private bool IncrementRefCount(string key)
    {
        if (_refCounts.TryGetValue(key, out int count))
        {
            _refCounts[key] = count + 1;
            return _loadedHandles.ContainsKey(key);
        }

        _refCounts[key] = 1;
        return false;
    }
    
    public void Dispose() => CleanUp();
}