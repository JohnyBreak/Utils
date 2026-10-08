using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.CollisionStrategies
{
    public class LineCastStrategy : IBulletCollisionStrategy
    {
        public RaycastHit[] CastCollision(IBullet bullet)
        {
            Vector3 rayDirection = bullet.GetCurrentPosition() - bullet.GetPreviousPosition();
            float rayLength = rayDirection.magnitude;

            if (rayLength <= 0.001f) return System.Array.Empty<RaycastHit>();

            var hits = Physics.RaycastAll(bullet.GetPreviousPosition(), rayDirection.normalized, rayLength);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            return hits;
        }
    }
}