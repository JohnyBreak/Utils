using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime
{
    public class ItemDefinition : IItemDefinition
    {
        private readonly IReadOnlyList<IItemComponent> _components = new List<IItemComponent>();

        public ItemDefinition(List<IItemComponent> components)
        {
            _components = components;
        }

        public IReadOnlyList<IItemComponent> Components => _components;

        public bool Has<T>() where T : class, IItemComponent
        {
            foreach (var component in _components)
            {
                if (component is T)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGet<T>(out IItemComponent component) where T : class, IItemComponent
        {
            foreach (var innerComponent in _components)
            {
                if (innerComponent is T target)
                {
                    component = target;
                    return true;
                }
            }

            component = new EmptyComponent();
            return false;
        }
    }
}