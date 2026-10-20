using System;

namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public class BulletProvider : IBulletProvider
    {
        private readonly Func<BulletSpawnInfo, IBullet> _get;
        private readonly Action<IBullet> _release;

        public BulletProvider(
            Func<BulletSpawnInfo, IBullet> get,
            Action<IBullet> release)
        {
            _get = get;
            _release = release;
        }

        public IBullet Get(BulletSpawnInfo spawnInfo)
        {
            return _get?.Invoke(spawnInfo);
        }

        public void Release(IBullet bullet)
        {
            _release?.Invoke(bullet);
        }
    }
}