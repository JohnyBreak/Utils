using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime.ActionHandlers
{
    public interface IActionHandler
    {
        bool TryExecute(IItem item, IItemComponent component);
    }
}