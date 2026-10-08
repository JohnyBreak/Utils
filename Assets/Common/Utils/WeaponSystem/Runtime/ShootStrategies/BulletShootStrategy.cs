namespace Common.Utils.WeaponSystem.Runtime.ShootStrategies
{
    public class BulletShootStrategy : IShootStrategy
    {
        private readonly IShootAdapter _adapter;

        public BulletShootStrategy(IShootAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Shoot(ShotInfo shotInfo)
        {
            _adapter.OnShot(shotInfo);
        }
    }
}