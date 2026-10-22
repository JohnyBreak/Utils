using Common.Utils.WeaponSystem.Runtime.Ammo;
using Common.Utils.WeaponSystem.Runtime.Magazine;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;
using Common.Utils.WeaponSystem.Runtime.ShootStrategies;

namespace Common.Utils.WeaponSystem.Runtime
{
    public class ModularWeapon : IWeapon
    {
        private readonly IShootStrategy _shootStrategy;
        private readonly IReloadStrategy _reloadStrategy;
        private readonly IWeaponView _view;
        private IMagazine _magazine;
        
        public ModularWeapon(
            IShootStrategy shootStrategy,
            IReloadStrategy reloadStrategy,
            IMagazine magazine,
            IWeaponView view)
        {
            _shootStrategy = shootStrategy;
            _reloadStrategy = reloadStrategy;
            _magazine = magazine;
            _view = view;
        }
        
        public void TryUse()
        {
            if (!_magazine.CanUse())
            {
                return;
            }
            
            _shootStrategy?.Shoot(
                _view.GetShootPosition(),
                _magazine);
        }

        public void TryReload()
        {
            _reloadStrategy?.Reload(_magazine);
        }
    }
}