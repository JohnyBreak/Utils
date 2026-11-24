using System;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;

namespace Bootstrap
{
    public class DefinitionsCreator
    {
        private readonly ISummerInfoGetter _infoGetter;
        
        public DefinitionsCreator(ISummerInfoGetter infoGetter)
        {
            _infoGetter = infoGetter;
        }

        public Dictionary<Type, SummerModuleDefinition> CreateDefinitions(List<IExecutableModule> sortedModules)
        {
            Dictionary<Type, SummerModuleDefinition> definitions =
                new Dictionary<Type, SummerModuleDefinition>(sortedModules.Count);
            
            for (int i = 0; i < sortedModules.Count; i++)
            {
                if(!_infoGetter.TryGetStageType(sortedModules[i], out var stageType))
                {
                    throw new SummerModuleException($"У модуля {sortedModules[i].GetType()} нет стадии");
                }
                
                if(!_infoGetter.TryGetStageIndex(sortedModules[i], out var stageIndex))
                {
                    throw new SummerModuleException($"У модуля {sortedModules[i].GetType()} нет стадии");
                }
                
                if(!_infoGetter.TryGetOrder(sortedModules[i], out var order))
                {
                    throw new SummerModuleException($"У модуля {sortedModules[i].GetType()} нет ордера");
                }
                
                if(!_infoGetter.TryGetDependencies(sortedModules[i], out var dependencies))
                {
                    dependencies = Array.Empty<Type>();
                }
                
                var definition = new SummerModuleDefinition(stageType, stageIndex, order, dependencies);
                definitions[sortedModules[i].GetType()] = definition;
                
            }

            return definitions;
        }
    }
}