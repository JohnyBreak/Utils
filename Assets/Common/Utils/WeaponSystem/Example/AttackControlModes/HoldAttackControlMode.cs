using System;
using System.Collections;
using Common.Utils.Equipment.Runtime;
using Common.Utils.WeaponSystem.Example.Strategies;
using Common.Utils.WeaponSystem.Runtime.AttackControlMode;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;
using Common.Utils.WeaponSystem.Runtime.Components;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.AttackControlModes
{
    public class HoldAttackControlMode: IAttackControlMode, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;
        private readonly IAttackStrategy _strategy;
        private readonly MonoBehaviour _coroutineProvider;
        private readonly IFireRateValidator _fireRateValidator;
        private Coroutine _coroutine;
        private float _lastShotTime = -999;
        
        public HoldAttackControlMode(
            IInputAdapter inputAdapter,
            IWeaponEquipment equipment,
            IAttackStrategy strategy,
            MonoBehaviour coroutineProvider,
            IFireRateValidator fireRateValidator)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
            _strategy = strategy;
            _coroutineProvider = coroutineProvider;
            _fireRateValidator = fireRateValidator;
        }
        
        private void Attack()
        {
            StopAttack();
            _coroutine = _coroutineProvider.StartCoroutine(ShootRoutine());
        }

        //TODO можно вынести всю эту логику в стратегию стрельбы, ведь это только control mode, сделать IStoppableAttackStrategy и работать через него
        private IEnumerator ShootRoutine() 
        {
            //TODO можно закэшировать для оптимизации
            // подписаться на смену оружия и кэшировать на смене
            
            var weapon = _weaponEquipment.GetCurrentWeapon();
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

        private void StopAttack()
        {
            if (_coroutine != null)
            {
                _coroutineProvider.StopCoroutine(_coroutine);
            }
        }
        
        public void Dispose()
        {
            Deactivate();
        }

        public void Activate()
        {
            _inputAdapter.AttackPressedEvent += Attack;
            _inputAdapter.AttackReleasedEvent += StopAttack;
        }

        public void Deactivate()
        {
            _inputAdapter.AttackPressedEvent -= Attack;
            _inputAdapter.AttackReleasedEvent -= StopAttack;
        }
    }
}