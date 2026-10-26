using System;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Runtime.Components;

namespace Common.Utils.Equipment.Runtime
{
    public class Equipment : IWeaponEquipment
    {
        public event Action<IItem> WeaponEquippedEvent;
        
        private IItem _currentWeapon;

        public IItem GetCurrentWeapon()
        {
            return _currentWeapon;
        }
        
        public void EquipWeapon(IItem weapon)
        {
            if (!weapon.HasComponent<IWeaponComponent>())
            {
                return;
            }

            //check if it is weapon
            _currentWeapon = weapon;
            WeaponEquippedEvent?.Invoke(weapon);
        }
    }
}