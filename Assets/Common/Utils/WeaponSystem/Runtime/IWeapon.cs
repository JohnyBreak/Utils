namespace Common.Utils.WeaponSystem.Runtime
{
    public interface IWeapon
    {
        bool CanUse();
        void TryUse();
        void TryReload();
    }
}