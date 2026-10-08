using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.CollisionStrategies
{
    public interface IBulletCollisionStrategy
    {
        RaycastHit[] CastCollision(IBullet bullet);
    }
}