using System;
using Common.Utils.Equipment.Runtime;
using Common.Utils.WeaponSystem.Runtime.AttackControlMode;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;

namespace Common.Utils.WeaponSystem.Example.AttackControlModes
{
    public class HoldAttackControlMode: IAttackControlMode, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _equipment;
        private readonly IStoppableAttackStrategy _strategy;

        public HoldAttackControlMode(
            IInputAdapter inputAdapter,
            IWeaponEquipment equipment,
            IStoppableAttackStrategy strategy)
        {
            _inputAdapter = inputAdapter;
            _equipment = equipment;
            _strategy = strategy;
        }
        
        private void Attack()
        {
            var item = _equipment.GetCurrentWeapon();
            
            _strategy.Attack(item);
        }
        
        private void StopAttack()
        {
            _strategy.Stop();
        }
        
        public void Dispose()
        {
            StopAttack();
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