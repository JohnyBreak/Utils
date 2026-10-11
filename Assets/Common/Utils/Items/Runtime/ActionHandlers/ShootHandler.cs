using Common.Utils.Items.Runtime.Components;
using Common.Utils.Items.Runtime.States;
using UnityEngine;

namespace Common.Utils.Items.Runtime.UseHandlers
{
    public class ShootHandler : IActionHandler
    {
        public bool TryExecute(IItem item, IItemComponent component)
        {
            if (!(component is ShootItemComponent shootItemComponent))
            {
                return false;
            }

            if (!item.TryGetState<ShootItemComponent>(out var state))
            {
                return false;
            }

            if (!(state is ShootItemState shootState))
            {
                return false;
            }

            if(!shootItemComponent.CanUse(shootState))
            {
                return false;
            }

            shootState.AmoAmount--;
            Debug.LogError(shootState.AmoAmount);
            return true;
        }
    }
}