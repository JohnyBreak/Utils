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
            
            if (!item.TryGetComponent<ShootComponent>(out var outComponent))
            {
                return false;
            }

            ShootComponent shootComponent = (ShootComponent)outComponent;

            BulletConfig def = GetCurrentBulletDef();
            int amount = shootComponent.MagSize;
            int realAmount = RequestBullets(def, amount);
            
            if (realAmount < 1)
            {
                return false;
            }

            shootComponent.Amo = realAmount;
            
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