using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.ActionHandlers;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Test
{
    public class ReloadHandler : IActionHandler
    {
        public bool TryExecute(IItem item, IItemComponent component)
        {
            if (!(component is ReloadItemComponent reloadItemComponent))
            {
                return false;
            }

            if(!reloadItemComponent.CanUse())
            {
                return false;
            }
            
            
            BulletConfig def = reloadItemComponent.GetBulletInfo();
            int amount = reloadItemComponent.GetMagSize();
            int realAmount = RequestBullets(def, amount);
            
            if (realAmount < 1)
            {
                return false;
            }

            reloadItemComponent.Reload(realAmount);
            
            return true;
        }

        private int RequestBullets(BulletConfig def, int amount)
        {
            //у инвентаря запросить кол-во патронов определенного типа,
            //инвентарь вернет либо сколько надо, либо сколько есть
            return amount;
        }
    }
}