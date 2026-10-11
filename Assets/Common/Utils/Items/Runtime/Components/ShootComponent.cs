using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.External;
using UnityEngine;

namespace Common.Utils.Items.Runtime.Components
{
    public class ShootItemComponent : IUseComponent
    {
        private readonly GunData _gunData;
        
        public ShootItemComponent(GunData gunData)
        {
            _gunData = gunData;
        }

        public bool CanUse()
        {
            return _gunData.CanShoot();
        }

        public void Shoot()
        {
            _gunData.Shoot();
        }
        
        public BulletConfig GetBulletDef()
        {
            return _gunData.GetBulletInfo();
        }
        
        public Transform GetShootPosition()
        {
            return _gunData.GetShootPosition();
        }
    }
}