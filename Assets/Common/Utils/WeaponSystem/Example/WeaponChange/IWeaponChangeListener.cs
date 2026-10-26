using Common.Utils.Items.Runtime;

namespace Common.Utils.WeaponSystem.Example.WeaponChange
{
    public interface IWeaponChangeListener
    {
        void Fire(IItem item);
    }
}