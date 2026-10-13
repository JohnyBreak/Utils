using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime
{
    public class Item : IItem
    {
        private readonly List<IItemComponent> _components = new List<IItemComponent>();

        public Item(List<IItemComponent> components)
        {
            foreach (var component in components)
            {
                ValidateComponent(component);
            }
            
            _components = components;
        }

        public bool HasComponent<T>() where T : class, IItemComponent
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

        public bool TryGetComponent<T>(out IItemComponent component) where T : class, IItemComponent
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

        public IReadOnlyList<IItemComponent> GetComponents()
        {
            return _components;
        }

        public void AddComponent(IItemComponent component)
        {
            ValidateComponent(component);

            _components?.Add(component);
        }

        public bool TryRemoveComponent(IItemComponent component)
        {
            ValidateComponent(component);
            
            return _components.Remove(component);
        }

        private void ValidateComponent(IItemComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }
        }
    }
}