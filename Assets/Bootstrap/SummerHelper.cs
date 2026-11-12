using System;
using System.Collections.Generic;

namespace Bootstrap
{
    public static class SummerHelper
    {
        public static bool HasSummerAttributes<T>(object obj) where T : Attribute
        {
            var result = obj.GetType().GetCustomAttributes(typeof(T), false);
            return result.Length > 0;
        }
        
        public static List<SummerSettingsAttribute> GetSummerAttributes(object obj)
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