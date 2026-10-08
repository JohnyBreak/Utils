using Common.Utils.Damageable;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.DamageStrategies
{
    public class SimpleDamageStrategy : IBulletDamageDeliveryStrategy
    {
        public void DeliverDamage(RaycastHit[] hits, IBullet bullet)
        {
            if (hits == null || hits.Length == 0)
            {
                return;
            }

            RaycastHit closestHit = hits[0];

            if (closestHit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(bullet.GetDamage());
            }
            
            bullet.Expire();
        }
    }
}