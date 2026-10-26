using System;
using Common.Utils.Items.Runtime;

namespace Common.Utils.Equipment.Runtime
{
    public interface IWeaponEquipment
    {
        event Action<IItem> WeaponEquippedEvent;
        void EquipWeapon(IItem weapon);
        IItem GetCurrentWeapon();
    }
}