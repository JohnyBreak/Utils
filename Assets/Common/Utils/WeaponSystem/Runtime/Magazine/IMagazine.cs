using Common.Utils.WeaponSystem.Runtime.Ammo;

namespace Common.Utils.WeaponSystem.Runtime.Magazine
{
    public interface IMagazine
    {
        void Consume();
        bool CanUse();
        bool TryReload(IAmmo ammo);
        bool TrySetAmmo(IAmmo ammo);
    }
}