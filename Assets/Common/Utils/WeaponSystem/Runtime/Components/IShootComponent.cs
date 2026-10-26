namespace Common.Utils.WeaponSystem.Runtime.Components
{
    public interface IShootComponent : IAttackComponent
    {
        float GetFireRate();
    }
}