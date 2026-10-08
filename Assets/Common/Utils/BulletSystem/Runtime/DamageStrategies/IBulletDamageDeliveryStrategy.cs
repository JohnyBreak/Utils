using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.DamageStrategies
{
    public interface IBulletDamageDeliveryStrategy
    {
        void DeliverDamage(RaycastHit[] hits, IBullet bullet);
    }
}