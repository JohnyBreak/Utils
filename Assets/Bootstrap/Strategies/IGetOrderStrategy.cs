namespace Bootstrap.Strategies
{
    public interface IGetOrderStrategy
    {
        bool TryGetOrder(object obj, out int order);
    }
}