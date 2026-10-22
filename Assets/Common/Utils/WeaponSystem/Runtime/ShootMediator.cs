using Common.Utils.BulletSystem.Runtime;

namespace Common.Utils.WeaponSystem.Runtime
{
    public class BulletShootAdapter : IShootAdapter
    {
        private readonly IBulletSystem _bulletSystem; 

        public BulletShootAdapter(
            IBulletSystem bulletSystem)
        {
            _bulletSystem = bulletSystem;
        }

        public void OnShot(ShotInfo info)
        {
            BulletInfo bulletInfo = BulletInfo.ManyBulletShot(
                info.ShotPosition,
                info.Direction,
                info.Config,
                info.BulletAmount);
            _bulletSystem.HandleBullet(bulletInfo);
        }
    }
}