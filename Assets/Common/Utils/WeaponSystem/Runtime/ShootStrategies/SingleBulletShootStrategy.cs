using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Example.AttackComponents;
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

        public void Shoot(Vector3 shotPosition, IItem item)
        {
            if (!item.TryGetComponent<BulletMagazine>(out var magazineComponent))
            {
                return;
            }
            // создать recoilService чтобы корректировать разброс
            // при смене оружия разброс сбрасывается
            BulletMagazine magazine = (BulletMagazine)magazineComponent;
            
            if (!magazine.HasAmmo())
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
                magazine.GetBulletConfig());
            
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