using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.Actions;

namespace Common.Utils.Items.Example
{
    public class WeaponProxy
    {
        private readonly Equipment _equipment;
        private readonly ItemActionService _actionService;
        
        public WeaponProxy(
            Equipment equipment, 
            ItemActionService actionService)
        {
            _actionService = actionService;
            _equipment = equipment;
        }

        public void Use()
        {
            var item = _equipment.GetCurrentWeapon();
            _actionService.TryExecute<UseAction>(item);
        }

        public void Reload()
        {
            var item = _equipment.GetCurrentWeapon();
            _actionService.TryExecute<ReloadAction>(item);
        }
    }
}