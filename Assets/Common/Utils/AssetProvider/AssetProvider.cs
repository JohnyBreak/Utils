using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetProvider
{
    private readonly Dictionary<string, AsyncOperationHandle> m_CompletedCache = new();
    private readonly Dictionary<string, List<AsyncOperationHandle>> m_Handles = new();
    private readonly Dictionary<string, List<GameObject>> m_SpawnedObjects = new();

    public async Task<T> LoadAssetAsync<T>(AssetReference reference) where T : class
    {
        return await LoadAssetAsync<T>(reference.AssetGUID);
    }

    public async Task<T> LoadAssetAsync<T>(string key) where T : class
    {
        if (m_CompletedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
        {
            return completedHandle.Result as T;
        }

        AsyncOperationHandle<T> enquedOperationHandle = Addressables.LoadAssetAsync<T>(key);

        enquedOperationHandle.Completed += completedOperationHandle =>
        {
            CompleteHandle(
                key,
                completedOperationHandle,
                enquedOperationHandle);
        };

        AddHandle(key, enquedOperationHandle);

        return await enquedOperationHandle.Task;
    }

    public void CleanUp()
    {
        foreach (var pair in m_SpawnedObjects)
        {
            foreach (var asset in pair.Value)
            {
                ReleaseAsset(pair.Key, asset);
            }
        }

        m_SpawnedObjects.Clear();

        foreach (List<AsyncOperationHandle> handlesList in m_Handles.Values)
        {
            foreach (AsyncOperationHandle handle in handlesList)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
        }

        m_Handles.Clear();

        foreach (AsyncOperationHandle asyncOperationHandle in m_CompletedCache.Values)
        {
            if (asyncOperationHandle.IsValid())
            {
                Addressables.Release(asyncOperationHandle);
            }
        }

        m_CompletedCache.Clear();
    }

    public async Task<GameObject> InstantiateAsync(AssetReference assetReference)
    {
        return await InstantiateAsync(assetReference.AssetGUID);
    }

    public async Task<GameObject> InstantiateAsync(string key)
    {
        if (m_CompletedCache.ContainsKey(key) == false)
        {
            await LoadAssetAsync<GameObject>(key);
        }

        var instanceHandle = Addressables.InstantiateAsync(key);
        GameObject obj = await instanceHandle.Task;
        if (!m_SpawnedObjects.TryGetValue(key, out List<GameObject> gameObjectList))
        {
            gameObjectList = new List<GameObject>();
            m_SpawnedObjects[key] = gameObjectList;
        }

        m_SpawnedObjects[key].Add(obj);

        return obj;
    }

    public void ReleaseAsset(AssetReference assetReference, GameObject assetObject)
    {
        ReleaseAsset(assetReference.AssetGUID, assetObject);
    }

    public void Release(AssetReference assetReference)
    {
        Release(assetReference.AssetGUID);
    }

    public void Release(string key)
    {
        if (m_CompletedCache.TryGetValue(key, out AsyncOperationHandle operation))
        {
            Addressables.Release(operation);
            m_CompletedCache.Remove(key);
        }
    }

    public void ReleaseAsset(string key, GameObject assetObject)
    {
        Addressables.ReleaseInstance(assetObject);
        m_SpawnedObjects[key].Remove(assetObject);
        if (m_SpawnedObjects[key].Count == 0)
        {
            if (m_CompletedCache.ContainsKey(key) == false)
            {
                return;
            }

            if (m_CompletedCache[key].IsValid() == false)
            {
                return;
            }

            var completedHandle = m_CompletedCache[key];
            Addressables.Release(completedHandle);
            m_CompletedCache.Remove(key);
            m_Handles.Remove(key);
        }
    }

    private void AddHandle<T>(string key, AsyncOperationHandle<T> operationHandle) where T : class
    {
        if (!m_Handles.TryGetValue(key, out List<AsyncOperationHandle> handleList))
        {
            handleList = new List<AsyncOperationHandle>();
            m_Handles[key] = handleList;
        }

        handleList.Add(operationHandle);
    }

    private void CompleteHandle<T>(string key, AsyncOperationHandle<T> completedOperationHandle,
        AsyncOperationHandle<T> enquedOperationHandle) where T : class
    {
        m_CompletedCache[key] = completedOperationHandle;
        List<AsyncOperationHandle> handlesList = m_Handles[key];
        if (handlesList != null && handlesList.Count > 0)
        {
            handlesList.Remove(enquedOperationHandle);
        }
    }

    public void OnDestroy()
    {
        CleanUp();
    }
}