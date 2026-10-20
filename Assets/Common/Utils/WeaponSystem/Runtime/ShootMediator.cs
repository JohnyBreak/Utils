using Common.Utils.BulletSystem.Runtime;

namespace Common.Utils.WeaponSystem.Runtime
{
    public class BulletShootAdapter : IShootAdapter
    {
        private readonly IBulletSystem _bulletSystem; 

        private BulletShootAdapter(
            IBulletSystem bulletSystem)
        {
            _bulletSystem = bulletSystem;
        }

        public void OnShot(ShotInfo info)
        {
            for (int i = 0; i < info.BulletAmount; i++)
            {
                BulletSpawnInfo bulletSpawnInfo = new BulletSpawnInfo(
                    info.ShotPosition,
                    info.Direction,
                    info.Config);
                _bulletSystem.HandleBullet(bulletSpawnInfo);
            }
        }
    }
}