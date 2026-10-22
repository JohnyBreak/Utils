using Common.Utils.WeaponSystem.Example.Test.Pistol;
using Common.Utils.WeaponSystem.Runtime.Ammo;
using Common.Utils.WeaponSystem.Runtime.Magazine;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.ShootStrategies
{
    public class SingleBulletShootStrategy : IShootStrategy
    {
        private readonly IShootAdapter _adapter;
        private readonly IAimDirection _aimDirection;

        public SingleBulletShootStrategy(
            IShootAdapter adapter,
            IAimDirection aimDirection)
        {
            _adapter = adapter;
            _aimDirection = aimDirection;
        }

        public void Shoot(Vector3 shotPosition, IMagazine magazine)
        {
            if (magazine is not BulletMagazine bulletMagazine)
            {
                return;
            }
            // создать recoilService чтобы корректировать разброс
            // при смене оружия разброс сбрасывается
            
            // сделать показатель скорострельности, чтобы был промежуток между выстрелами, чтобы не закликать моментально
            if (!magazine.CanUse())
            {
                return;
            }
            
            ShotInfo shotInfo = ShotInfo.SingleShot(
                shotPosition,
                _aimDirection.GetDirection(),
                bulletMagazine.GetConfig());
            
            _adapter.OnShot(shotInfo);
            
            magazine.Consume();
        }
    }
}