using System;
using Common.Utils.Items.Runtime;

namespace Common.Utils.Equipment.Runtime
{
    public class Equipment : IWeaponEquipment
    {
        private IItem _currentWeapon;

        public IItem GetCurrentWeapon()
        {
            return _currentWeapon;
        }

        public event Action<Item> WeaponEquippedEvent;
        
        public void EquipWeapon(Item weapon)
        {
            //check if it is weapon
            _currentWeapon = weapon;
            WeaponEquippedEvent?.Invoke(weapon);
        }
    }
}