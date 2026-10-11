using System.Collections.Generic;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime
{
    public interface IItemDefinition
    {
        IReadOnlyList<IItemComponent> Components { get; }
        bool Has<T>() where T : class, IItemComponent;
        bool TryGet<T>(out IItemComponent component) where T : class, IItemComponent;
    }
}