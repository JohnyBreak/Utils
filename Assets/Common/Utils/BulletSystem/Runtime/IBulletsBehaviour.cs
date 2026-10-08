namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBulletsBehaviour
    {
        void RegisterBullet(IBullet bullet);
        void Update(float deltaTime);
    }
}