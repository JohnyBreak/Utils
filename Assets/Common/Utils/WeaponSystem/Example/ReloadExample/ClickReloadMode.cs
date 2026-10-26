using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Equipment.Runtime;
using Common.Utils.Items.Example;
using Common.Utils.WeaponSystem.Example.AttackComponents;

namespace Common.Utils.WeaponSystem.Example.ReloadExample
{
    public class ClickReloadMode : IReloadMode
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly IWeaponEquipment _weaponEquipment;

        public ClickReloadMode(IInputAdapter inputAdapter, IWeaponEquipment equipment)
        {
            _inputAdapter = inputAdapter;
            _weaponEquipment = equipment;
        }
        
        private void Reload()
        {
            var item = _weaponEquipment.GetCurrentWeapon();
            
            if (!item.HasComponent<ReloadComponent>())
            {
                return;
            }
            
            if (!item.TryGetComponent<BulletMagazine>(out var magComponent))
            {
                return;
            }
            var mag = ((BulletMagazine)magComponent);
            // если 
            mag.Fill(RequestBullets(mag.GetBulletConfig(), mag.GetCapacity()));
        }

        public void Dispose()
        {
            Deactivate();
        }
        
        private int RequestBullets(BulletConfig def, int amount)
        {
            //у инвентаря запросить кол-во патронов определенного типа,
            //инвентарь вернет либо сколько надо, либо сколько есть
            return amount;
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