using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;
using Common.Utils.Items.Runtime.States;

namespace Common.Utils.Items.Runtime
{
    public class ItemInstance : IItem
    {
        private readonly IItemDefinition _definition;
        private readonly Dictionary<Type, IItemState> _states;

        public ItemInstance(IItemDefinition definition, Dictionary<Type, IItemState> states)
        {
            _definition = definition;
            _states = states;
        }

        public bool HasComponent<T>() where T : class, IItemComponent
        {
            return _definition.Has<T>();
        }
        
        public bool TryGetState<T>(out IItemState state) where T : IItemComponent
        {
            if (!_states.TryGetValue(typeof(T), out var innerState))
            {
                state = null;
                return false;
            }

            state = innerState;
            return true;
        }

        public bool TryGetComponent<T>(out IItemComponent component) where T : class, IItemComponent
        {
            return _definition.TryGet<T>(out component);
        }
        
        public IReadOnlyList<IItemComponent> GetComponents()
        {
            return _definition.Components;
        }
    }
}