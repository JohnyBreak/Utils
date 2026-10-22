using System;
using System.Collections.Generic;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.AttackerExample
{
    public class Attacker : IWeaponChangeListener
    {
        private readonly Dictionary<Type, IAttacker> _map;
        private IAttacker _current;
        
        public Attacker(Dictionary<Type, IAttacker> map)
        {
            _map = map;
        }

        public void Fire(Type key)
        {
            if (!_map.TryGetValue(key, out var attacker))
            {
                Debug.LogError($"No IAttacker for key {key}");
                return;
            }
            
            _current?.Deactivate();
            _current = attacker;
            _current?.Activate();
        }
    }
}