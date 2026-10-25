using Common.Utils.WeaponSystem.Example.Test.Pistol;
using Common.Utils.WeaponSystem.Runtime.Magazine;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.ShootStrategies
{
    public class SingleBulletShootStrategy : IShootStrategy
    {
        private readonly IShootAdapter _adapter;
        private readonly IAimDirection _aimDirection;
        private readonly float _fireRate;
        private float _lastShotTime = -999;
        
        public SingleBulletShootStrategy(
            IShootAdapter adapter,
            IAimDirection aimDirection,
            float fireRate)
        {
            _adapter = adapter;
            _aimDirection = aimDirection;
            _fireRate = fireRate;
        }

        public void Shoot(Vector3 shotPosition, IMagazine magazine)
        {
            if (magazine is not BulletMagazine bulletMagazine)
            {
                return;
            }
            // создать recoilService чтобы корректировать разброс
            // при смене оружия разброс сбрасывается
            
            if (!magazine.CanUse())
            {
                return;
            }

            if (!CanShootByRate())
            {
                return;
            }

            ShotInfo shotInfo = ShotInfo.SingleShot(
                shotPosition,
                _aimDirection.GetDirection(),
                bulletMagazine.GetConfig());
            
            _adapter.OnShot(shotInfo);
            
            magazine.Consume();

            _lastShotTime = Time.time;
        }

        private bool CanShootByRate()
        {
            var result = (Time.time - _lastShotTime) >= _fireRate;
            return result;
        }
    }
}