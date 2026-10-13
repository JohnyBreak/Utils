using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime
{
    public interface IItem
    {
        bool HasComponent<T>() where T : class, IItemComponent;
        bool TryGetComponent<T>(out IItemComponent component) where T : class, IItemComponent;
        IReadOnlyList<IItemComponent> GetComponents();

        void AddComponent(IItemComponent component);
        bool TryRemoveComponent(IItemComponent component);
    }
}