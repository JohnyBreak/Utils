using System;
using Common.Utils.Items.Runtime;

namespace Common.Utils.Equipment.Runtime
{
    public interface IWeaponEquipment
    {
        event Action<Item> WeaponEquippedEvent;
        void EquipWeapon(Item weapon);
        IItem GetCurrentWeapon();
    }
}