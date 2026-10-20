namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public interface IBulletProvider
    {
        IBullet Get(BulletSpawnInfo spawnInfo);
        void Release(IBullet bullet);
    }
}