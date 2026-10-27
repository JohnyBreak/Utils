using System.Collections.Generic;
using UnityEngine;

namespace Bootstrap
{
    public class Initializer : IInitializer
    {
        private readonly List<IOrdered> _ordereds;

        public Initializer(List<IOrdered> ordereds)
        {
            _ordereds = ordereds;
        }

        public void Start()
        {
            Sort(_ordereds);
            
            foreach (var ordered in _ordereds)
            {
                ordered.StartExecute();
            }
            
            Debug.Log("complete");
        }

        private void Sort(List<IOrdered> ordereds)
        {
            ordereds.Sort((left, right) =>
            {
                var leftAttributes = GetSummerAttributes(left);
                var rightAttributes = GetSummerAttributes(right);

                if (leftAttributes.Count < 1)
                {
                    return 1;
                }

                if (rightAttributes.Count < 1)
                {
                    return -1;
                }
                
                return leftAttributes[0].Order - rightAttributes[0].Order;
            });
        }

        private List<SummerSettingsAttribute> GetSummerAttributes(object obj)
        {
            List<SummerSettingsAttribute> result = new List<SummerSettingsAttribute>();
            
            var customAttributes = obj.GetType().GetCustomAttributes(typeof(SummerSettingsAttribute), false);
                
            foreach (var customAttribute in customAttributes)
            {
                if(customAttribute is SummerSettingsAttribute summerSettings)
                {
                    result.Add(summerSettings);
                }
            }

            return result;
        }
    }
}