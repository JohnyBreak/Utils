using Common.Utils.Items.Runtime.States;

namespace Common.Utils.Items.Runtime.Components
{
    public class ShootItemComponent : IUseComponent
    {
        private readonly int _magSize;
        public ShootItemComponent(int magSize)
        {
            _magSize = magSize;
        }

        public bool CanUse(IItemState state)
        {
            if (!(state is ShootItemState shootState))
            {
                return false;
            }

            return shootState.AmoAmount > 0;
        }
    }
}