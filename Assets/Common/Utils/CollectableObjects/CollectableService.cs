using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectables.UI;
using Collectables.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Collectables
{
    public class CollectableService
    {
        private readonly CollectableResolver _resolver;
        private readonly CollectableFloatingTextCanvas _canvas;
        private readonly CollectableFactory _factory;
        private List<CollectableConfig> _configs;

        public CollectableService(
            AssetProvider assetProvider,
            TestCollectableContainer container,
            CollectableFloatingTextCanvas canvas)
        {
            _resolver = new CollectableResolver(new List<ICollector>()
            {
                new PistolAmmoCollector(container)
            });
            
            _canvas = canvas;
            _factory = new CollectableFactory(assetProvider, OnEnter, OnExit, OnCollect);
            
            FillConfigs();
        }

        public async UniTask SpawnCollectable(CollectableConfig lootEntryConfig, Vector3 lootEntryPosition)
        {
            await _factory.SpawnObjectAt(lootEntryConfig, lootEntryPosition);
        }

        private void OnEnter(CollectableObjectView view)
        {
            if (_canvas == null)
            {
                return;
            }
            _canvas.Show(view);
        }
        
        private void OnExit(CollectableObjectView view)
        {
            if (_canvas == null)
            {
                return;
            }
            _canvas.Hide(view);
        }
        
        private void OnCollect(CollectableObjectView view, Action onSuccess, Action onFail)
        {
            if (_resolver.TryCollect(view.Config))
            {
                _canvas.Hide(view);
                onSuccess?.Invoke();
            }
            else
            {
                if (_canvas != null)
                {
                    _canvas.SetRedText(view);
                }
                
                onFail?.Invoke();
            }
        }

        private void FillConfigs()// парсить с json
        {
            _configs = new List<CollectableConfig>();
        }
    }
}