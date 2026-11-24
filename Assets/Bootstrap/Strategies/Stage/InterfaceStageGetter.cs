using System;

namespace Bootstrap.Strategies
{
    public class InterfaceStageGetter : IGetStageStrategy
    {
        private readonly Func<object, Type, int> _getStageIndex;

        public InterfaceStageGetter(Func<object, Type, int> getStageIndex)
        {
            _getStageIndex = getStageIndex;
        }
        
        public bool TryGetStageIndex(object obj, out int index)
        {
            if (!Accepted(obj))
            {
                index = 0;
                return false;
            }
            
            index = _getStageIndex.Invoke(obj, ((ISummerModule)obj).GetStage());
            return true;
        }

        public bool TryGetStageType(object obj, out Type stageType)
        {
            if (!Accepted(obj))
            {
                stageType = null;
                return false;
            }

            stageType = ((ISummerModule)obj).GetStage();
            return true;
        }

        private bool Accepted(object obj)
        {
            return obj is ISummerModule;
        }
    }
}