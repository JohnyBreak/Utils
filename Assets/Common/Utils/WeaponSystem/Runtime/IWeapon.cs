namespace Common.Utils.WeaponSystem.Runtime
{
    public interface IWeapon
    {
        
    }

    public interface IGun : IWeapon
    {
        bool CanUse();
        void TryUse();
        void TryReload();
    }
}