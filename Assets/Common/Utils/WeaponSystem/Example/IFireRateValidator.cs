namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public interface IFireRateValidator
    {
        bool CanShoot(float lastShotTime, float fireRate);
    }
}