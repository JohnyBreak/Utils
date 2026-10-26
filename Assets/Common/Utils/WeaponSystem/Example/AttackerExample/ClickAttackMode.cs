using System;
using Common.Utils.Equipment.Runtime;

namespace Common.Utils.WeaponSystem.Example.AttackerExample
{
    public class ClickAttackMode : IAttackMode, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;

        public ClickAttackMode(IInputAdapter inputAdapter, IWeaponEquipment equipment)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
        }
        
        private void Attack()
        {
            //_weaponEquipment.GetCurrentWeapon();
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