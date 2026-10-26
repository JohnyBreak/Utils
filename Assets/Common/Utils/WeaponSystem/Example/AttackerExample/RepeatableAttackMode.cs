using System;
using System.Collections;
using Common.Utils.Equipment.Runtime;
using Common.Utils.WeaponSystem.Runtime;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.AttackerExample
{
    public class RepeatableAttackMode: IAttackMode, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;
        private readonly float _fireRate;
        private readonly MonoBehaviour _coroutineProvider;
        private Coroutine _coroutine;
        
        public RepeatableAttackMode(
            IInputAdapter inputAdapter,
            IWeaponEquipment equipment,
            float fireRate,
            MonoBehaviour coroutineProvider)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
            _fireRate = fireRate;
            _coroutineProvider = coroutineProvider;
        }
        
        private void Attack()
        {
            StopAttack();
            _coroutine = _coroutineProvider.StartCoroutine(ShootRoutine());
            // start coroutine
            //_weapon.TryUse();
        }

        private IEnumerator ShootRoutine()
        {
            yield break;
            //while (true)
            //{
            //    yield return new WaitForSeconds(_fireRate);

            //    if (!_weapon.CanUse())
            //    {
            //        yield break;
            //    }
            //    
            //    _weapon.TryUse();
            //}
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