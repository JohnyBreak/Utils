namespace Bootstrap.Strategies
{
    public interface IGetStageIndexStrategy
    {
        bool TryGetStageIndex(object obj, out int index);
    }
}