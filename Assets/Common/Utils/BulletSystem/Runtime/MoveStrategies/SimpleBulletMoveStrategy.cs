using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.MoveStrategies
{
    public class SimpleBulletMoveStrategy : IBulletMoveStrategy
    {
        public void Move(IBullet bullet, float deltaTime)
        {
            bullet.UpdatePreviousPosition();
            Vector3 displacement = bullet.GetVelocity() * deltaTime;
            bullet.SetCurrentPosition(bullet.GetPreviousPosition() + displacement);
            bullet.AddTraveledDistance(displacement.magnitude);
        }
    }
}