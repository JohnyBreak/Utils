using System;
using Common.Utils.WeaponSystem.Runtime;

namespace Common.Utils.WeaponSystem.Example.AttackerExample
{
    public class ClickAttacker : IAttacker, IDisposable
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeapon _weapon;

        public ClickAttacker(IInputAdapter inputAdapter, IWeapon weapon)
        {
            _inputAdapter = inputAdapter;
            _weapon = weapon;
        }
        
        private void Attack()
        {
            _weapon.TryUse();
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