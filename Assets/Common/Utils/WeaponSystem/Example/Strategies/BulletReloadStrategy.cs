using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.WeaponSystem.Runtime.Components;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;

namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public class BulletReloadStrategy : IReloadStrategy
    {
        public void Reload(IItem item)
        {
            if (!item.HasComponent<IReloadComponent>())
            {
                return;
            }
            
            if (!item.TryGetComponent<BulletMagazine>(out var magComponent))
            {
                return;
            }
            //TODO у перезарядки должно быть время, запихнуть в компонент
            var mag = ((BulletMagazine)magComponent);
            mag.Fill(RequestBullets(mag.GetBulletConfig(), mag.GetCapacity()));
        }
        
        private int RequestBullets(BulletConfig def, int amount)
        {
            //TODO
            //у инвентаря запросить кол-во патронов определенного типа,
            //инвентарь вернет либо сколько надо, либо сколько есть
            return amount;
        }
    }
}