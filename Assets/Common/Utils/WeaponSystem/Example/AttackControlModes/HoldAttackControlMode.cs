using System;
using System.Collections;
using Common.Utils.Equipment.Runtime;
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
        private Coroutine _coroutine;
        
        public HoldAttackControlMode(
            IInputAdapter inputAdapter,
            IWeaponEquipment equipment,
            IAttackStrategy strategy,
            MonoBehaviour coroutineProvider)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
            _strategy = strategy;
            _coroutineProvider = coroutineProvider;
        }
        
        private void Attack()
        {
            StopAttack();
            _coroutine = _coroutineProvider.StartCoroutine(ShootRoutine());
        }

        private IEnumerator ShootRoutine()//TODO усли просто кликать как пистолетм, то ничего не происходит :/
        {
            var weapon = _weaponEquipment.GetCurrentWeapon();
            if (!weapon.TryGetComponent<IShootComponent>(out var shootComponent))
            {
                yield break;
            }
            
            IShootComponent shoot = (IShootComponent)shootComponent;

            while (true)
            {
                yield return new WaitForSeconds(shoot.GetFireRate());

                _strategy.Attack(weapon);
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