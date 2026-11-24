using System;

namespace Bootstrap
{
    public class SummerModuleDefinition
    {
        public readonly Type[] Dependencies;
        public Type StageType { get; }
        public int StageIndex { get; }
        public int Order { get; }

        public SummerModuleDefinition(Type stageType, int stageIndex, int order, Type[] dependencies = null)
        {
            StageIndex = stageIndex;
            Dependencies = dependencies;
            StageType = stageType;
            Order = order;
        }
    }
}