using System.Collections;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;
using Common.Utils.WeaponSystem.Runtime.Components;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public class AutoBulletShootStrategy : IStoppableAttackStrategy
    {
        private readonly IAttackStrategy _strategy;
        private readonly MonoBehaviour _coroutineProvider;
        private readonly IFireRateValidator _fireRateValidator;
        private Coroutine _coroutine;
        private float _lastShotTime = -999;

        public AutoBulletShootStrategy(
            IAttackStrategy strategy,
            MonoBehaviour coroutineProvider,
            IFireRateValidator fireRateValidator)
        {
            _strategy = strategy;
            _coroutineProvider = coroutineProvider;
            _fireRateValidator = fireRateValidator;
        }

        public void Attack(IItem item)
        {
            if (!item.HasComponent<IShootComponent>())
            {
                return;
            }
            
            Stop();
            _coroutine = _coroutineProvider.StartCoroutine(ShootRoutine(item));
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                _coroutineProvider.StopCoroutine(_coroutine);
            }
        }
        
        private IEnumerator ShootRoutine(IItem item) 
        {
            //TODO можно закэшировать для оптимизации
            // подписаться на смену оружия и кэшировать на смене
            
            var weapon = item;
            if (!weapon.TryGetComponent<IShootComponent>(out var shootComponent))
            {
                yield break;
            }
            
            IShootComponent shoot = (IShootComponent)shootComponent;

            while (true)
            {
                if (!_fireRateValidator.CanShoot(_lastShotTime, shoot.GetFireRate()))
                {
                    yield return null;
                }

                _strategy.Attack(weapon);
                
                _lastShotTime = Time.time;
            }
        }
    }
}