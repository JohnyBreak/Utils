using Common.Utils.BulletSystem.Runtime.BulletProvider;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletSystem : IBulletSystem
    {
        private readonly IBulletProvider _bulletProvider;
        private readonly IBulletBehaviour _bulletBehaviour;

        public BulletSystem(
            IBulletProvider bulletProvider,
            IBulletBehaviour bulletBehaviour)
        {
            _bulletProvider = bulletProvider;
            _bulletBehaviour = bulletBehaviour;
        }

        public void HandleBullet(BulletSpawnInfo spawnInfo)
        {
            var bullet = _bulletProvider.Get(spawnInfo);
            _bulletBehaviour.RegisterBullet(bullet);
        }
        
        public void Update(float deltaTime)
        {
            _bulletBehaviour.Update(deltaTime);
        }
    }
}