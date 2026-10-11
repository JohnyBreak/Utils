using Common.Utils.Items.Runtime.Actions;

namespace Common.Utils.Items.Runtime
{
    public interface IItemActionService
    {
        bool TryExecute<TAction>(IItem item) where TAction : IItemAction;
    }
}