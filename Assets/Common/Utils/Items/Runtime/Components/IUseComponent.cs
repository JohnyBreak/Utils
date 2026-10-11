using Common.Utils.Items.Runtime.States;

namespace Common.Utils.Items.Runtime.Components
{
    public interface IUseComponent : IItemComponent
    {
        bool CanUse(IItemState state);
    }
}