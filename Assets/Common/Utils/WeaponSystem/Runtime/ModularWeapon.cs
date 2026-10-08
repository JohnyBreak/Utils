using Common.Utils.BulletSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;
using Common.Utils.WeaponSystem.Runtime.ShootStrategies;

namespace Common.Utils.WeaponSystem.Runtime
{
    public class ModularWeapon : IWeapon
    {
        private readonly IShootStrategy _shootStrategy;
        private readonly IReloadStrategy _reloadStrategy;
        private readonly IWeaponView _view;
        private BulletConfig _config;
        
        public ModularWeapon(
            IShootStrategy shootStrategy,
            IReloadStrategy reloadStrategy,
            IWeaponView view,
            BulletConfig defaultConfig)
        {
            _shootStrategy = shootStrategy;
            _reloadStrategy = reloadStrategy;
            _view = view;
            _config = defaultConfig;
        }

        public void SetBulletConfig(BulletConfig config)
        {
            _config = config;
        }
        
        public void TryShoot()
        {
            var info = ShotInfo.SimpleShot(_view.GetShootPosition(), _config);
            _shootStrategy?.Shoot(info);
        }

        public void TryReload()
        {
            _reloadStrategy?.Reload();
        }
    }
}