using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Runtime.UseHandlers
{
    public interface IActionHandler
    {
        bool TryExecute(IItem item, IItemComponent component);
    }
}