namespace Bootstrap.Strategies
{
    public class AttributeOrderGetter : IGetOrderStrategy
    {
        public bool TryGetOrder(object obj, out int order)
        {
            if (!Accepted(obj))
            {
                order = 0;
                return false;
            }

            var innerOrder = GetOrder(obj);
            
            order = innerOrder;
            return true;
        }
        
        private bool Accepted(object obj)
        {
            return SummerHelper.HasSummerAttributes<SummerSettingsAttribute>(obj);
            // было вот так, но почему-то тесты проходили правильно, думаю, должно было падать return obj is ISummerModule;
        }
        
        private int GetOrder(object obj)
        {
            var attributes = SummerHelper.GetSummerAttributes(obj);
            
            var result = attributes[0].Order;
            
            return result;
        }
    }
}