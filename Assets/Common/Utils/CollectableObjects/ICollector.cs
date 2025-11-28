namespace Collectables
{
    public interface ICollector
    {
        bool TryCollect(CollectableConfig collectable);
        int GetCollectableType();
    }
}


