using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using Common.Utils.WeaponSystem.Runtime.Components;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.AttackControlMode
{
    public class CurrentAttackMode : IWeaponChangeListener
    {
        private readonly Dictionary<Type, IAttackControlMode> _map;
        private IAttackControlMode _current;
        
        public CurrentAttackMode(Dictionary<Type, IAttackControlMode> map)
        {
            _map = map;
        }

        public void Fire(IItem item)
        {
            if (!item.TryGetComponent<IAttackComponent>(out var component))
            {
                return;
            }

            if (!_map.TryGetValue(component.GetType(), out var attacker))
            {
                Debug.LogError($"No Attack mode for key {component.GetType()}");
                return;
            }
            
            _current?.Deactivate();
            _current = attacker;
            _current?.Activate();
        }
    }
}