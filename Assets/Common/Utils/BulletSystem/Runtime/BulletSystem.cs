using Common.Utils.BulletSystem.Runtime.BulletProvider;
using Common.Utils.WeaponSystem.Runtime;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletSystem : IBulletSystem
    {
        private readonly IBulletProvider _bulletProvider;
        private readonly IBulletsBehaviour _bulletsBehaviour;

        public BulletSystem(
            IBulletProvider bulletProvider,
            IBulletsBehaviour bulletsBehaviour)
        {
            _bulletProvider = bulletProvider;
            _bulletsBehaviour = bulletsBehaviour;
        }

        public void HandleShot(ShotInfo info)
        {
            var bullet = _bulletProvider.Get(info.Config, info.ShotPosition, info.Direction);
            _bulletsBehaviour.RegisterBullet(bullet);
        }
        
        public void Update(float deltaTime)
        {
            _bulletsBehaviour.Update(deltaTime);
        }
    }
}