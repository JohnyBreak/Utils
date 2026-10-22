using Common.Utils.WeaponSystem.Runtime.Ammo;
using Common.Utils.WeaponSystem.Runtime.Magazine;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.ShootStrategies
{
    public interface IShootStrategy
    {
        void Shoot(Vector3 shotPosition, IMagazine magazine);
    }
}