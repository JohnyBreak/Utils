using Common.Utils.Items.Runtime;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.ShootStrategies
{
    public interface IShootStrategy
    {
        void Shoot(Vector3 shotPosition, IItem item);
    }
}