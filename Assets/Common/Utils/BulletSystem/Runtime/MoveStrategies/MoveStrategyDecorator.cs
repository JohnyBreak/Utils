using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.MoveStrategies
{
    public class MoveStrategyDecorator : IBulletMoveStrategy
    {
        private readonly Dictionary<BulletType, IBulletMoveStrategy> _strategies;

        public MoveStrategyDecorator(Dictionary<BulletType, IBulletMoveStrategy> strategies)
        {
            _strategies = strategies;
        }

        public void Move(IBullet bullet, float deltaTime)
        {
            if (!_strategies.TryGetValue(bullet.GetBulletType(), out var strategy))
            {
                Debug.LogError($"No move strategy for type {bullet.GetType()}. bullet is expired");
                bullet.Expire();
                return;
            }

            strategy.Move(bullet, deltaTime);
        }
    }
}