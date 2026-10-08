using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.DamageStrategies
{
    public class DamageStrategyDecorator : IBulletDamageDeliveryStrategy
    {
        private readonly Dictionary<BulletType, IBulletDamageDeliveryStrategy> _strategies;

        public DamageStrategyDecorator(Dictionary<BulletType, IBulletDamageDeliveryStrategy> strategies)
        {
            _strategies = strategies;
        }

        public void DeliverDamage(RaycastHit[] hits, IBullet bullet)
        {
            if (!_strategies.TryGetValue(bullet.GetBulletType(), out var strategy))
            {
                Debug.LogError($"No damage strategy for type {bullet.GetType()}. bullet is expired");
                bullet.Expire();
                return;
            }

            strategy.DeliverDamage(hits, bullet);
        }
    }
}