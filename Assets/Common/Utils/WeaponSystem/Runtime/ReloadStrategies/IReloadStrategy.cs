using Common.Utils.WeaponSystem.Runtime.Magazine;

namespace Common.Utils.WeaponSystem.Runtime.ReloadStrategies
{
    public interface IReloadStrategy
    {
        void Reload(IMagazine magazine);
    }
}