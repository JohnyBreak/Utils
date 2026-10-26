using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;
using Common.Utils.WeaponSystem.Runtime.Components;
using Common.Utils.WeaponSystem.Runtime.Helpers;
using Common.Utils.WeaponSystem.Runtime.MediatorAdapters;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public class SingleBulletShootStrategy : IAttackStrategy
    {
        private readonly IShootAdapter _adapter;
        private readonly IAimDirection _aimDirection;
        private readonly IshootPositionProvider _positionProvider;
        private float _lastShotTime = -999;
        
        public SingleBulletShootStrategy(
            IShootAdapter adapter,
            IAimDirection aimDirection,
            IshootPositionProvider positionProvider)
        {
            _adapter = adapter;
            _aimDirection = aimDirection;
            _positionProvider = positionProvider;
        }

        public void Attack(IItem item)
        {
            if (!item.TryGetComponent<IShootComponent>(out var shootComponent))
            {
                return;
            }
            
            if (!item.TryGetComponent<BulletMagazine>(out var magazineComponent))
            {
                return;
            }
            // создать recoilService чтобы корректировать разброс
            // при смене оружия разброс сбрасывается
            BulletMagazine magazine = (BulletMagazine)magazineComponent;
            IShootComponent shoot = (IShootComponent)shootComponent;
            
            if (!magazine.HasAmmo())
            {
                return;
            }

            if (!CanShootByRate(shoot.GetFireRate()))
            {
                return;
            }

            ShotInfo shotInfo = ShotInfo.SingleShot(
                _positionProvider.Get(),
                _aimDirection.GetDirection(),
                magazine.GetBulletConfig());
            
            _adapter.OnShot(shotInfo);
            
            magazine.Consume();

            _lastShotTime = Time.time;
        }

        private bool CanShootByRate(float fireRate)
        {
            var result = (Time.time - _lastShotTime) >= fireRate;
            return result;
        }
    }
}