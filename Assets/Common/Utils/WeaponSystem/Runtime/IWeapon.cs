using Common.Utils.BulletSystem.Runtime;

namespace Common.Utils.WeaponSystem.Runtime
{
    public interface IWeapon
    {
        void TryShoot();
        void TryReload();
        void SetBulletConfig(BulletConfig config);
    }
}