using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime.Actions;
using Common.Utils.Items.Runtime.UseHandlers;

namespace Common.Utils.Items.Runtime
{
    public class ItemActionActionService : IItemActionService
    {
        private Dictionary<(Type, Type), IActionHandler> _handlers = new Dictionary<(Type, Type), IActionHandler>();
        
        public ItemActionActionService(Dictionary<(Type, Type), IActionHandler> handlers)
        {
            _handlers = handlers;
        }

        public bool TryExecute<TAction>(IItem item) where TAction : IItemAction
        {
            if (item == null)
            {
                return false;
            }
            var action = typeof(TAction);
            foreach (var component in item.GetComponents())
            {
                if (!_handlers.TryGetValue((action, component.GetType()), out var handler))
                {
                    continue;
                }
                
                return handler.TryExecute(item, component);
            }

            return false;
        }
    }
}