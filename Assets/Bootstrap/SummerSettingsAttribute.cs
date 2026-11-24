using System;
using System.Collections.Generic;

namespace Bootstrap
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SummerSettingsAttribute : Attribute
    {
        public readonly Type[] Dependencies;
        public Type StageType { get; }
        public int Order { get; }

        public SummerSettingsAttribute(Type stageType, int order, Type[] dependencies = null)
        {
            Dependencies = dependencies;
            StageType = stageType;
            Order = order;
        }
    }
}