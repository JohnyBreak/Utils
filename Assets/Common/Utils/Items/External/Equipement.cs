using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.Actions;

namespace Common.Utils.Items.External
{
    public class Equipment
    {
        private IItem _current;
        
        public void Equip(IItem item)
        {
            _current = item;
        }

        public IItem GetCurrentWeapon()
        {
            return _current;
        }
    }

    public class WeaponProxy
    {
        private readonly Equipment _equipment;
        private readonly ItemActionActionService _actionActionService;
        
        public WeaponProxy(
            Equipment equipment, 
            ItemActionActionService actionActionService)
        {
            _actionActionService = actionActionService;
            _equipment = equipment;
        }

        public void Use()
        {
            var item = _equipment.GetCurrentWeapon();
            _actionActionService.TryExecute<UseAction>(item);
        }

        public void Reload()
        {
            var item = _equipment.GetCurrentWeapon();
            _actionActionService.TryExecute<ReloadAction>(item);
        }
    }

}