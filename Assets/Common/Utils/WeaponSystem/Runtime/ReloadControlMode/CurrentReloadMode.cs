using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using Common.Utils.WeaponSystem.Runtime.Components;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.ReloadControlMode
{
    public class CurrentReloadMode : IWeaponChangeListener
    {
        private readonly Dictionary<Type, IReloadControlMode> _map;
        private IReloadControlMode _current;
        
        public CurrentReloadMode(Dictionary<Type, IReloadControlMode> map)
        {
            _map = map;
        }

        public void Fire(IItem item)
        {
            if (!item.TryGetComponent<IReloadComponent>(out var component))
            {
                return;
            }

            if (!_map.TryGetValue(component.GetType(), out var reloader))
            {
                Debug.LogError($"No IReloader for key {component.GetType()}");
                return;
            }
            
            _current?.Deactivate();
            _current = reloader;
            _current?.Activate();
        }
    }
}