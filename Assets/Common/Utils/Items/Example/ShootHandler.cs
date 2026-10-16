using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.ActionHandlers;
using Common.Utils.Items.Runtime.Components;
using UnityEngine;

namespace Common.Utils.Items.Example
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
            if (!(component is AmmoComponent shootItemComponent))
            {
                return false;
            }

            if(!shootItemComponent.CanUse())
            {
                return false;
            }

            var def = GetCurrentBulletDef();
            var position = GetShootPosition();

            // TODO взять откуда-нибудь из aim сервиса куда персонаж целится
            var direction = GetDirection();
            
            
            _adapter.Shoot(def, position, direction);
            //мб надо тут проверить отработал ли адаптер, если да, то вызывать шут у компонента
            shootItemComponent.Consume();
            return true;
        }
        
        private BulletConfig GetCurrentBulletDef()
        {
            return new BulletConfig();
        }

        private Vector3 GetShootPosition()
        {
            return new Vector3();
        }
        
        private Vector3 GetDirection()
        {
            return new Vector3();
        }
    }
}