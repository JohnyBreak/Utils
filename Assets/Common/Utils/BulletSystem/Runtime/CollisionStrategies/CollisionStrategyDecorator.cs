using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.CollisionStrategies
{
    public class CollisionStrategyDecorator : IBulletCollisionStrategy
    {
        private readonly Dictionary<BulletType, IBulletCollisionStrategy> _strategies;

        public CollisionStrategyDecorator(Dictionary<BulletType, IBulletCollisionStrategy> strategies)
        {
            _strategies = strategies;
        }

        public RaycastHit[] CastCollision(IBullet bullet)
        {
            if (!_strategies.TryGetValue(bullet.GetBulletType(), out var strategy))
            {
                Debug.LogError($"No collision strategy for type {bullet.GetType()}. bullet is expired");
                bullet.Expire();
                return Array.Empty<RaycastHit>();
            }

            return strategy.CastCollision(bullet);
        }
    }
}