using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;
using Common.Utils.WeaponSystem.Runtime.Components;
using Common.Utils.WeaponSystem.Runtime.Helpers;
using Common.Utils.WeaponSystem.Runtime.MediatorAdapters;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public class ShotgunShootStrategy : IAttackStrategy
    {
        private readonly IShootAdapter _adapter;
        private readonly IAimDirection _aimDirection;
        private readonly IshootPositionProvider _positionProvider;
        private readonly IFireRateValidator _fireRateValidator;
        private readonly System.Random _rng = new();
        private float _lastShotTime = -999;
        public ShotgunShootStrategy(
            IShootAdapter adapter,
            IAimDirection aimDirection,
            IshootPositionProvider positionProvider,
            IFireRateValidator fireRateValidator)
        {
            _adapter = adapter;
            _aimDirection = aimDirection;
            _positionProvider = positionProvider;
            _fireRateValidator = fireRateValidator;
        }

        public void Attack(IItem item)
        {
            if (!item.TryGetComponent<ShotgunShootComponent>(out var shootComponent))
            {
                return;
            }
            
            if (!item.TryGetComponent<BulletMagazine>(out var magazineComponent))
            {
                return;
            }
            // создать recoilService чтобы корректировать разброс ну или вынести это в другое место, типа сделать отдачу, которая влияет на прицеливание
            // при смене оружия разброс сбрасывается
            
            BulletMagazine magazine = (BulletMagazine)magazineComponent;
            ShotgunShootComponent shotgun = (ShotgunShootComponent)shootComponent;
            
            if (!magazine.HasAmmo())
            {
                return;
            }

            if (!_fireRateValidator.CanShoot(_lastShotTime, shotgun.GetFireRate()))
            {
                return;
            }

            Shoot(
                shotgun.GetBulletsPerShot(), 
                magazine.GetBulletConfig(), 
                shotgun.GetSpreadAngle());
            
            magazine.Consume();

            _lastShotTime = Time.time;
        }

        private void Shoot(int bulletsAmount, BulletConfig config, float spreadAngle)
        {
            var direction = _aimDirection.GetDirection();
            
            for (int i = 0; i < bulletsAmount; i++)
            {
                Vector3 pelletDir = ApplyGaussianSpread(direction, spreadAngle);
                ShotInfo shotInfo = ShotInfo.SingleShot(
                    _positionProvider.Get(),
                    pelletDir,
                    config);
            
                _adapter.OnShot(shotInfo);
            }
        }
        
        private Vector3 ApplyGaussianSpread(Vector3 direction, float spreadAngleDegrees)
        {
            // Box-Muller для нормального распределения
            double u1 = 1.0 - _rng.NextDouble();
            double u2 = _rng.NextDouble();
            double gaussian = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Cos(2.0 * System.Math.PI * u2);

            // Масштабируем: sigma = spreadAngle / 3, чтобы 99.7% попадало в конус
            float angleOffset = (float)gaussian * (spreadAngleDegrees / 3f) * Mathf.Deg2Rad;

            // Случайный азимут
            float phi = (float)_rng.NextDouble() * Mathf.PI * 2f;
            float sinT = Mathf.Sin(Mathf.Abs(angleOffset));
            float cosT = Mathf.Cos(angleOffset);

            Vector3 localDir = new Vector3(sinT * Mathf.Cos(phi), sinT * Mathf.Sin(phi), cosT);
            return Quaternion.LookRotation(direction, Vector3.up) * localDir;
        }
    }
}