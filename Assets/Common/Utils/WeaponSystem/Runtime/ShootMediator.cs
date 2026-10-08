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
            _bulletSystem.HandleShot(info);
        }
    }
}