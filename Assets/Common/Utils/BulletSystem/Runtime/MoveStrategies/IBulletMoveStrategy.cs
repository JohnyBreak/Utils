namespace Common.Utils.BulletSystem.Runtime.MoveStrategies
{
    public interface IBulletMoveStrategy
    {
        void Move(IBullet bullet, float deltaTime);
    }
}