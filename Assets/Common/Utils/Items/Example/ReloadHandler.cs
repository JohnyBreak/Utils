using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.ActionHandlers;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Example
{
    public class ReloadHandler : IActionHandler
    {
        public bool TryExecute(IItem item, IItemComponent component)
        {
            if (!(component is ReloadComponent))
            {
                return false;
            }
            
            if (!item.TryGetComponent<AmmoComponent>(out var outComponent))
            {
                return false;
            }

            AmmoComponent ammoComponent = (AmmoComponent)outComponent;

            BulletConfig def = GetCurrentBulletDef();
            int amount = ammoComponent.MagSize;
            int realAmount = RequestBullets(def, amount);
            
            if (realAmount < 1)
            {
                return false;
            }

            ammoComponent.Ammo = realAmount;
            
            return true;
        }

        private int RequestBullets(BulletConfig def, int amount)
        {
            //у инвентаря запросить кол-во патронов определенного типа,
            //инвентарь вернет либо сколько надо, либо сколько есть
            return amount;
        }

        private BulletConfig GetCurrentBulletDef()
        {
            return new BulletConfig();
        }
    }
}