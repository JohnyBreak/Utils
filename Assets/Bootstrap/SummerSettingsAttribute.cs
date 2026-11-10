using System;

namespace Bootstrap
{
    //нам нужна возможность добавлять новые этапы
    public class MainStage
    {
        
    }

    //stage
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SummerSettingsAttribute : Attribute
    {
        public Type StageType { get; }
        public int Order { get; }

        public SummerSettingsAttribute(Type stageType, int order)
        {
            StageType = stageType;
            Order = order;
        }
    }
}