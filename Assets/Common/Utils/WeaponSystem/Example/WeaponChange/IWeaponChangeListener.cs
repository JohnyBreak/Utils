using System;

namespace Common.Utils.WeaponSystem.Example.WeaponChange
{
    public interface IWeaponChangeListener
    {
        void Fire(Type key);
    }
}