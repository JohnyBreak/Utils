using System;

namespace Bootstrap.Strategies
{
    public interface IGetStageStrategy
    {
        bool TryGetStageIndex(object obj, out int index);
        bool TryGetStageType(object obj, out Type stageType);
    }
}