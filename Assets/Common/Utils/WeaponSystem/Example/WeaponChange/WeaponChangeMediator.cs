using System;
using System.Collections.Generic;
using Common.Utils.Equipment.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Example.AttackControlModes;
using Common.Utils.WeaponSystem.Runtime.Components;

namespace Common.Utils.WeaponSystem.Example.WeaponChange
{
    public class WeaponChangeMediator : IDisposable
    {
        private readonly IWeaponEquipment _equipment;
        private readonly List<IWeaponChangeListener> _listeners;

        public WeaponChangeMediator(
            IWeaponEquipment equipment, 
            List<IWeaponChangeListener> listeners)
        {
            _equipment = equipment;
            _listeners = listeners;

            _equipment.WeaponEquippedEvent += OnWeaponChanged;
        }
        
        private void OnWeaponChanged(IItem weapon)
        {
            if (!weapon.HasComponent<IWeaponComponent>())
            {
                return;
            }
            
            foreach (var weaponChangeListener in _listeners)
            {
                weaponChangeListener.Fire(weapon);
            }
        }

        public void Dispose()
        {
            _equipment.WeaponEquippedEvent -= OnWeaponChanged;
        }
    }
}