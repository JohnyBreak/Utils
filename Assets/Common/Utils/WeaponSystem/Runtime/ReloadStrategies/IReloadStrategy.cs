using Common.Utils.Items.Runtime;

namespace Common.Utils.WeaponSystem.Runtime.ReloadStrategies
{
    public interface IReloadStrategy
    {
        void Reload(IItem magazine);
    }
}