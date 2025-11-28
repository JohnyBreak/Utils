using System;

namespace Collectables.View
{
    public interface ICollectableView
    {
        void Init(CollectableConfig collectable,
            Action<CollectableObjectView> onEnter,
            Action<CollectableObjectView> onExit,
            Action<CollectableObjectView, Action, Action> onCollect);
        void Collect();
    }
}