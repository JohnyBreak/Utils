using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime
{
    public class ItemInstance : IItem
    {
        private readonly IItemDefinition _definition;

        public ItemInstance(IItemDefinition definition)
        {
            _definition = definition;
        }

        public bool HasComponent<T>() where T : class, IItemComponent
        {
            return _definition.Has<T>();
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