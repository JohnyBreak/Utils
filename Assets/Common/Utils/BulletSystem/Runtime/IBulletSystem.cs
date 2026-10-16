namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBulletSystem
    {
        void HandleBullet(BulletInfo info);
        void Update(float deltaTime);
    }
}