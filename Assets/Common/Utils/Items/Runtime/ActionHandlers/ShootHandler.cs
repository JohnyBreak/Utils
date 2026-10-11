using Common.Utils.Items.External;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime.UseHandlers
{
    public class ShootHandler : IActionHandler
    {
        private readonly IShootAdapter _adapter;

        public ShootHandler(IShootAdapter adapter)
        {
            _adapter = adapter;
        }

        public bool TryExecute(IItem item, IItemComponent component)
        {
            if (!(component is ShootItemComponent shootItemComponent))
            {
                return false;
            }

            if(!shootItemComponent.CanUse())
            {
                return false;
            }

            var def = shootItemComponent.GetBulletDef();
            var position = shootItemComponent.GetShootPosition();

            // TODO взять откуда-нибудь из aim сервиса куда персонаж целится
            var direction = position.forward;
            
            
            _adapter.Shoot(def, position.position, direction);
            //мб надо тут проверить отработал ли адаптер, если да, то вызывать шут у компонента
            shootItemComponent.Shoot();
            return true;
        }
    }
}