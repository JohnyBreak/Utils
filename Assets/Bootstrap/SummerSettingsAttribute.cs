using System;

namespace Bootstrap
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SummerSettingsAttribute : Attribute
    {
        public int Order { get; }

        public SummerSettingsAttribute(int order)
        {
            Order = order;
        }
    }
}