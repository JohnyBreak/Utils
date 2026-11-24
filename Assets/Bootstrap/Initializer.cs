using System;
using System.Collections;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Validation;
using UnityEngine;

namespace Bootstrap
{
    /*
     *sync
     * async
     *sync
     *
     */
                    
                    
    /*
     *
     *
     *
     */
    
    public enum SummerStatus
    {
        None = 0,
        Success,
        Failed,
        Error
    }

    public class Initializer : IInitializer
    {
        private readonly int _retryNumber = 3;

        private Dictionary<Type, SummerModuleDefinition> _definitions =
            new Dictionary<Type, SummerModuleDefinition>();
        
        private Dictionary<Type, ModuleDecorator> _decorators =
            new Dictionary<Type, ModuleDecorator>();
        
        private readonly List<IExecutableModule> _sortedModules;
        private readonly DefinitionsCreator _definitionsCreator;
        private readonly ISummerValidator _validator;
        private bool _isComplete;
        
        public Initializer(
            List<IExecutableModule> modules,
            ISummerValidator validator,
            DefinitionsCreator definitionsCreator,
            int retryNumber)
        {
            _sortedModules = modules;
            _validator = validator;
            _definitionsCreator = definitionsCreator;
            _retryNumber = retryNumber;
        }

        public IEnumerator Start()
        {
            Prepare();
            
            foreach (var module in _sortedModules)
            {
                if (!IsDependenciesReady(module))
                {
                    yield return null;
                }

                {
                    var statusHolder = new PromiseStatusHolder();
                    
                    yield return ExecuteModule(module, _retryNumber, statusHolder);

                    if (statusHolder.Status == SummerStatus.Error)
                    {
                        throw new SummerModuleException($"{module.GetType()} Module Execution Error. Initialization stopped");
                    }
                    
                    if (statusHolder.Status == SummerStatus.Failed)
                    {
                        // есть ли зарегистрированная заглушка
                        
                        // если нет заглушки, значит что-то критическое => кидаем эксепшн
                    }
                }
            }

            _isComplete = true;
            Debug.Log("complete");
        }
        
        public bool IsComplete()
        {
            return _isComplete;
        }
        
        private void Prepare()
        {
            _validator.Validate(_sortedModules);
            
            _definitions = _definitionsCreator.CreateDefinitions(_sortedModules);
            
            Sort(_sortedModules);
            
            Decorate(_sortedModules);
        }
        
        private bool IsDependenciesReady(IExecutableModule module)
        {
            if (module is not ModuleDecorator)
            {
                throw new SummerModuleException($"Нет декоратора ModuleDecorator для {module.GetType()}");
            }

            var definition = GetDefinition(((ModuleDecorator)module).GetOriginalType());

            if (definition.Dependencies == null)
            {
                return true;
            }

            if (definition.Dependencies.Length < 1)
            {
                return true;
            }

            foreach (var dependency in definition.Dependencies)
            {
                if (!_decorators.TryGetValue(dependency, out var decorator))
                {
                    throw new SummerModuleException($"Нет декоратора ModuleDecorator для {dependency}");
                }

                if (!decorator.IsInitialized())
                {
                    return false;
                }
            }

            return true;
        }
        
        private IEnumerator ExecuteModule(IExecutableModule ordered, int retryNumber, PromiseStatusHolder holder)
        {
            for (int i = 0; i < retryNumber; i++)
            {
                var promise = ordered.Execute();
                
                while (!promise.IsCompleted())
                {
                    yield return null;
                }

                var status = promise.GetStatus();

                if (status != SummerStatus.Error)
                {
                    holder.Status = promise.GetStatus();
                    yield break;
                }
            }

            holder.Status = SummerStatus.Error;
        }
        
        private void Decorate(List<IExecutableModule> sortedModules)
        {
            for (int i = 0; i < sortedModules.Count; i++)
            {
                var decorator = new ModuleDecorator(sortedModules[i]);

                _decorators[sortedModules[i].GetType()] = decorator;
                
                sortedModules[i] = decorator;
            }
        }
        
        private void Sort(List<IExecutableModule> modules)
        {
            modules.Sort(
                (left, right) =>
                {
                    var stagesResult = GetStageIndex(left).CompareTo(GetStageIndex(right));
                    
                    if(stagesResult != 0)
                    {
                        return stagesResult;
                    }
                    
                    return GetOrder(left).CompareTo(GetOrder(right));
                });
        }

        private int GetStageIndex(object obj)
        {
            var definition = GetDefinition(obj);
            
            return definition.StageIndex;
        }

        private int GetOrder(object obj)
        {
            var definition = GetDefinition(obj);

            return definition.Order;
        }

        private SummerModuleDefinition GetDefinition(object obj)
        {
            if (!_definitions.TryGetValue(obj.GetType(), out var definition))
            {
                throw new SummerModuleException($"Для модуля {obj.GetType()} нет зарегистрированного definition");
            }

            return definition;
        }
        private SummerModuleDefinition GetDefinition(Type type)
        {
            if (!_definitions.TryGetValue(type, out var definition))
            {
                throw new SummerModuleException($"Для модуля {type} нет зарегистрированного definition");
            }

            return definition;
        }
    }
}