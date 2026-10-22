using Common.Utils.WeaponSystem.Runtime;

namespace Common.Utils.WeaponSystem.Example.ReloadExample
{
    public class ClickReloader : IReloader
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeapon _weapon;

        public ClickReloader(IInputAdapter inputAdapter, IWeapon weapon)
        {
            _inputAdapter = inputAdapter;
            _weapon = weapon;
        }
        
        private void Reload()
        {
            _weapon.TryReload();
        }

        public void Dispose()
        {
            Deactivate();
        }

        public void Activate()
        {
            _inputAdapter.ReloadPressedEvent += Reload;
        }

        public void Deactivate()
        {
            _inputAdapter.ReloadPressedEvent -= Reload;
        }
    }
}