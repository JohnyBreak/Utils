using System;
using Common.Utils.Equipment.Runtime;
using Common.Utils.WeaponSystem.Runtime.AttackControlMode;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;

namespace Common.Utils.WeaponSystem.Example.AttackControlModes
{
    public class ClickAttackControlMode : IAttackControlMode, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;
        private readonly IAttackStrategy _strategy;

        public ClickAttackControlMode(
            IInputAdapter inputAdapter,
            IWeaponEquipment equipment,
            IAttackStrategy strategy)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
            _strategy = strategy;
        }
        
        private void Attack()
        {
            var weapon = _weaponEquipment.GetCurrentWeapon();
            _strategy.Attack(weapon);
        }

        public void Dispose()
        {
            Deactivate();
        }

        public void Activate()
        {
            _inputAdapter.AttackPressedEvent += Attack;
        }

        public void Deactivate()
        {
            _inputAdapter.AttackPressedEvent -= Attack;
        }
    }
}