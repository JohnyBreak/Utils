using Common.Utils.Equipment.Runtime;
using Common.Utils.WeaponSystem.Runtime.ReloadControlMode;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;

namespace Common.Utils.WeaponSystem.Example.ReloadControlMode
{
    public class ClickReloadControlMode : IReloadControlMode
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;
        private readonly IReloadStrategy _strategy;

        public ClickReloadControlMode(
            IInputAdapter inputAdapter, 
            IWeaponEquipment equipment,
            IReloadStrategy strategy)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
            _strategy = strategy;
        }
        
        public void Dispose()
        {
            Deactivate();
        }
        
        public void Activate()
        {
            _inputAdapter.ReloadPressedEvent += Reload;
        }

        private void Reload()
        {
            var weapon = _weaponEquipment.GetCurrentWeapon();
            _strategy.Reload(weapon);
        }

        public void Deactivate()
        {
            _inputAdapter.ReloadPressedEvent -= Reload;
        }
    }
}