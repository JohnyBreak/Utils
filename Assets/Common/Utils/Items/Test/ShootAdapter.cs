using Common.Utils.BulletSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime;
using UnityEngine;

namespace Common.Utils.Items.Test
{
    public class ShootAdapter : IShootAdapter
    {
        private readonly IBulletSystem _system;

        public ShootAdapter(IBulletSystem system)
        {
            _system = system;
        }

        public void Shoot(BulletConfig def, Vector3 position, Vector3 Direction)
        {
            _system.HandleShot(ShotInfo.SimpleShot(position,Direction, def));
        }
    }
}