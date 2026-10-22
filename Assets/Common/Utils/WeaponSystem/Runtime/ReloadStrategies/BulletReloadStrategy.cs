using Common.Utils.WeaponSystem.Example.Test.Pistol;
using Common.Utils.WeaponSystem.Runtime.Ammo;
using Common.Utils.WeaponSystem.Runtime.Magazine;

namespace Common.Utils.WeaponSystem.Runtime.ReloadStrategies
{
    public class BulletReloadStrategy : IReloadStrategy
    {
        public void Reload(IMagazine magazine)
        {
            if (magazine is not BulletMagazine bulletMagazine)
            {
                return;
            }
            
            bulletMagazine.TryReload(
                new BulletAmmo(
                bulletMagazine.GetConfig(),
                bulletMagazine.GetCapacity()));
        }
    }
}