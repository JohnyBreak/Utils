using Common.Utils.BulletSystem.Runtime.BulletProvider;
using Common.Utils.WeaponSystem.Runtime;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletSystem : IBulletSystem
    {
        private IBulletProvider _bulletProvider;
        private IBulletsBehaviour _bulletsBehaviour;

        public void HandleShot(ShotInfo info)
        {
            var bullet = _bulletProvider.Get(info.Config, info.ShotPosition);
            _bulletsBehaviour.RegisterBullet(bullet);
        }
        
        public void Update(float deltaTime)
        {
            _bulletsBehaviour.Update(deltaTime);
        }
    }
}