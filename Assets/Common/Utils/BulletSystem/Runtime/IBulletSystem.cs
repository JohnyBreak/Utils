namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBulletSystem
    {
        void HandleBullet(BulletSpawnInfo spawnInfo);
        void Update(float deltaTime);
    }
}