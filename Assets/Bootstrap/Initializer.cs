using System;
using System.Collections;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;
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

    // доделать тесты для зависимостей, начать писать преиниты
    public class Initializer : IInitializer // TODO сделать аналог preInit, init, postInit
    {
        private readonly int _retryNumber = 3;
        private Dictionary<Type, int> _stages = new Dictionary<Type, int>();

        private Dictionary<Type, SummerModuleDefinition> _definitions =
            new Dictionary<Type, SummerModuleDefinition>();

        private Dictionary<Type, ModuleDecorator> _decorators =
            new Dictionary<Type, ModuleDecorator>();

        private readonly List<IExecutableModule> _sortedModules;
        private readonly DefinitionsCreator _definitionsCreator;
        private readonly ISummerValidator _validator;
        private bool _isComplete;

        private const bool _log = true;
        private const bool _logDependencies = false;

        
        public Initializer(
            List<Type> stages,
            List<IExecutableModule> modules
        )
        {
            _stages = GetStagesMap(stages);
            _sortedModules = modules;
            var getter = CreateSummerInfoGetter();
            _validator = new SummerValidator(getter);
            _definitionsCreator = new DefinitionsCreator(getter);
        }

        public IEnumerator Start()
        {
            Prepare();

            yield return PreExecute();
            yield return Execute();
            yield return PostExecute();

            _isComplete = true;
            Log("complete");
        }

        public bool IsComplete()
        {
            return _isComplete;
        }

        private IEnumerator PreExecute()
        {
            foreach (var module in _sortedModules)
            {
                Log($"PreExecute модуля {((ModuleDecorator)module).GetOriginalType()}");
                module.PreExecute();
            }

            yield return null;
        }

        private IEnumerator Execute()
        {
            foreach (var module in _sortedModules)
            {
                while (!IsDependenciesReady(module))
                {
                    if (module is not ModuleDecorator)
                    {
                        throw new SummerModuleException(
                            $"Нет декоратора ModuleDecorator для {module.GetType()}");
                    }

                    if (_logDependencies)
                    {
                        LogDependencies($"Ждем зависимости модуля {((ModuleDecorator)module).GetOriginalType()}");
                    }
                    yield return null;
                }


                var statusHolder = new PromiseStatusHolder();

                Log($"Execute модуля {((ModuleDecorator)module).GetOriginalType()}");

                yield return ExecuteModule(module, _retryNumber, statusHolder);

                if (statusHolder.Status == SummerStatus.Error)
                {
                    throw new SummerModuleException(
                        $"{module.GetType()} Module Execution Error. Initialization stopped");
                }

                if (statusHolder.Status == SummerStatus.Failed)
                {
                    throw new SummerModuleException(
                        $"{module.GetType()} Module Execution Error. Initialization stopped");
                    // есть ли зарегистрированная заглушка

                    // если нет заглушки, значит что-то критическое => кидаем эксепшн
                }
            }
        }

        private IEnumerator PostExecute()
        {
            foreach (var module in _sortedModules)
            {
                Log($"PostExecute модуля {((ModuleDecorator)module).GetOriginalType()}");
                module.PostExecute();
            }

            yield return null;
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
                if (dependency == null)
                {
                    throw new SummerModuleException($"В списке зависимостей для {module.GetType()} есть null");
                }

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

        private IEnumerator PreExecuteModule(IExecutableModule module, int retryNumber, PromiseStatusHolder holder)
        {
            for (int i = 0; i < retryNumber; i++)
            {
                var promise = module.PreExecute();

                var status = promise.GetStatus();

                if (status != SummerStatus.Error)
                {
                    holder.Status = promise.GetStatus();
                    yield break;
                }
            }

            holder.Status = SummerStatus.Error;
        }

        private IEnumerator ExecuteModule(IExecutableModule module, int retryNumber, PromiseStatusHolder holder)
        {
            for (int i = 0; i < retryNumber; i++)
            {
                var promise = module.Execute();

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
            modules.Sort((left, right) =>
            {
                var stagesResult = GetStageIndex(left).CompareTo(GetStageIndex(right));

                if (stagesResult != 0)
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

        private Dictionary<Type, int> GetStagesMap(List<Type> list)
        {
            Dictionary<Type, int> stages = new Dictionary<Type, int>();

            for (int i = 0; i < list.Count; i++)
            {
                stages[list[i]] = i;
            }

            return stages;
        }

        private SummerInfoGetter CreateSummerInfoGetter()
        {
            var result = new SummerInfoGetter(
                new List<IGetStageStrategy>()
                {
                    new InterfaceStageGetter(GetStageIndex),
                    new AttributeStageGetter(GetStageIndex),
                },
                new List<IGetOrderStrategy>()
                {
                    new InterfaceOrderGetter(),
                    new AttributeOrderGetter(),
                },
                new List<IGetDependenciesStrategy>()
                {
                    new InterfaceDependenciesGetter(),
                    new AttributeDependenciesGetter(),
                });

            return result;
        }

        private int GetStageIndex(object obj, Type type)
        {
            if (type == null)
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} null стадия");
            }

            if (!_stages.TryGetValue(type, out var index))
            {
                throw new SummerModuleException($"У модуля {obj.GetType()} не зарегистрированная стадия {type}");
            }

            return index;
        }

        private void Log(string message)
        {
            if (!_log)
            {
                return;
            }
            
            Debug.Log(message);
        }
        private void LogDependencies(string message)
        {
            if (!_logDependencies)
            {
                return;
            }
            
            Debug.Log(message);
        }
    }
}