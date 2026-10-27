namespace Common.Utils.WeaponSystem.Runtime.AttackStrategies
{
    public interface IStoppableAttackStrategy : IAttackStrategy
    {
        void Stop();
    }
}