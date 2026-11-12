using System;
using System.Collections.Generic;
using Bootstrap.Exceptions;

namespace Bootstrap.Strategies
{
    public class AttributeStageIndexGetter : IGetStageIndexStrategy
    {
        private readonly Func<object, Type, int> _getStageIndex;

        public AttributeStageIndexGetter(Func<object, Type, int> getStageIndex)
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

            var type = GetStageType(obj);
            
            index = _getStageIndex.Invoke(obj, type);
            return true;
        }

        private Type GetStageType(object obj)
        {
            var attributes = SummerHelper.GetSummerAttributes(obj);
            
            if (attributes.Count < 1)
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} нет стадии");
            }

            var type = attributes[0].StageType;
            return type;
        }
        
        private bool Accepted(object obj)
        {
            return SummerHelper.HasSummerAttributes<SummerSettingsAttribute>(obj);
            // было вот так, но почему-то тесты проходили правильно, думаю, должно было падать return obj is ISummerModule;
        }
    }
}