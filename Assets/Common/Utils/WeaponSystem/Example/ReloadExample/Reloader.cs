using System;
using System.Collections.Generic;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.ReloadExample
{
    public class Reloader : IWeaponChangeListener
    {
        private readonly Dictionary<Type, IReloadMode> _map;
        private IReloadMode _current;
        
        public Reloader(Dictionary<Type, IReloadMode> map)
        {
            _map = map;
        }

        public void Fire(Type key)
        {
            if (!_map.TryGetValue(key, out var reloader))
            {
                Debug.LogError($"No IReloader for key {key}");
                return;
            }
            
            _current?.Deactivate();
            _current = reloader;
            _current?.Activate();
        }
    }
}