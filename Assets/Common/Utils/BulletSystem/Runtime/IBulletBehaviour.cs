namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBulletBehaviour
    {
        void RegisterBullet(IBullet bullet);
        void Update(float deltaTime);
    }
}