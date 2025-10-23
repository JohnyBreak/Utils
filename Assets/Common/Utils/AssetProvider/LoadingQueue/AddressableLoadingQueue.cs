using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetProvider.LoadingQueue
{
    public class AddressableLoadingQueue
    {
        private List<LoadingInfo> _loadingQueue = new();

        private AssetResult<LoadingInfo> GetLoadingInfo(IKeyEvaluator name)
        {
            int index = _loadingQueue.FindIndex(x => Equals(x.Key, name));
            if (index == -1)
            {
                return new AssetResult<LoadingInfo>(null, false);
            }

            return new AssetResult<LoadingInfo>(_loadingQueue[index], true);
        }

        public bool Contains(IKeyEvaluator loadingInfo)
        {
            return _loadingQueue.FindIndex(x => Equals(x.Key, loadingInfo)) != -1;
        }

        public void Enqueue(LoadingInfo info)
        {
            var bundleInfoResult = GetLoadingInfo(info.Key);
            if (!bundleInfoResult.IsExist)
            {
                _loadingQueue.Add(info);
            }
            else
            {
                Debug.LogError($"You have already added this bundle [{info.Key}] to load. You should call contains firstly");
            }
        }
        
        public void Remove(IKeyEvaluator assetId)
        {
            _loadingQueue.RemoveAll(x => Equals(x.Key, assetId));
        }
        
        public AssetResult<LoadingInfo> GetByIndex(int i)
        {
            if (i >= 0 && i < _loadingQueue.Count)
            {
                var bundleLoadHolder = _loadingQueue[i];
                return new AssetResult<LoadingInfo>(bundleLoadHolder, true);
            }

            return new AssetResult<LoadingInfo>(null, false);
        }
        
        public AssetResult<LoadingInfo> Dequeue()
        {
            if (_loadingQueue.Count > 0)
            {
                var last = 0;
                var bundleLoadHolder = _loadingQueue[last];
                _loadingQueue.RemoveAt(last);
                return new AssetResult<LoadingInfo>(bundleLoadHolder, true);
            }

            return new AssetResult<LoadingInfo>(null, false);
        }
        
        public int GetCount()
        {
            return _loadingQueue.Count;
        }

        public IDownloadHandler GetHandle(IKeyEvaluator assetId)
        {
            var bundleInfoResult = GetLoadingInfo(assetId);
            if (!bundleInfoResult.IsExist)
            {
                Debug.LogError("There is no any handler?");
                return default;
            }

            return bundleInfoResult.Object.Handler;
        }
    }
}