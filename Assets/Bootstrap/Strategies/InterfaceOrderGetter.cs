namespace Bootstrap.Strategies
{
    public class InterfaceOrderGetter : IGetOrderStrategy
    {
        public bool TryGetOrder(object obj, out int order)
        {
            if (!Accepted(obj))
            {
                order = 0;
                return false;
            }
            
            order = ((ISummerModule)obj).GetOrder();
            return true;
        }
        
        private bool Accepted(object obj)
        {
            return obj is ISummerModule;
        }
    }
}