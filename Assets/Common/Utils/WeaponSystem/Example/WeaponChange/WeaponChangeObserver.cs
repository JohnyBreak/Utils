using System;
using System.Collections.Generic;
using Common.Utils.Equipment.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Example.AttackComponents;
using Common.Utils.WeaponSystem.Example.AttackerExample;
using Common.Utils.WeaponSystem.Example.ReloadExample;

namespace Common.Utils.WeaponSystem.Example.WeaponChange
{
    public class WeaponChangeObserver : IDisposable
    {
        private readonly IWeaponEquipment _equipment;
        private readonly List<IWeaponChangeListener> _listeners;
        private readonly Attacker _attacker;
        private readonly Reloader _reloader;

        public WeaponChangeObserver(
            IWeaponEquipment equipment, 
            List<IWeaponChangeListener> listeners)
        {
            _equipment = equipment;
            _listeners = listeners;

            _equipment.WeaponEquippedEvent += OnWeaponChanged;
        }
        
        private void OnWeaponChanged(Item weapon)
        {
            if (!weapon.TryGetComponent<AttackTypeComponent>(out var component))
            {
                return;
            }
            
            foreach (var weaponChangeListener in _listeners)
            {
                weaponChangeListener.Fire(component.GetType());
            }
        }

        public void Dispose()
        {
            _equipment.WeaponEquippedEvent -= OnWeaponChanged;
        }
    }
}