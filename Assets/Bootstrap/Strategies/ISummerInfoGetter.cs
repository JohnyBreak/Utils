using System;

namespace Bootstrap.Strategies
{
    public interface ISummerInfoGetter
    {
        public bool TryGetStageIndex(object obj, out int index);
        public bool TryGetStageType(object obj, out Type stageType);
        public bool TryGetOrder(object obj, out int order);
        public bool TryGetDependencies(object obj, out Type[] dependencies);
    }
}