using Common.Utils.Items.Runtime;

namespace Common.Utils.WeaponSystem.Runtime.AttackStrategies
{
    public interface IAttackStrategy
    {
        void Attack(IItem item);
    }
}