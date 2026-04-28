using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppInit.InitSteps.Signals;
using Common.Utils.CoroutineProvider;
using Source.Scripts;
using UnityEngine;
using Zenject;

namespace AppInit.InitSteps
{
    public class StepIniter : IDisposable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IInitStepsProvider _stepsProvider;
        private readonly SignalBus _signalBus;
        private readonly Queue<int> _pendingGroups = new();
        private List<InitStepBase> _steps;
        private Coroutine _runRoutine;
        private bool _isStarted;
        private int _runningGroupId;

        public StepIniter(ICoroutineRunner coroutineRunner, IInitStepsProvider stepsProvider, SignalBus signalBus)
        {
            _coroutineRunner = coroutineRunner;
            _stepsProvider = stepsProvider;
            _signalBus = signalBus;
        }

        public void Run(int groupId)
        {
            Debug.Log($"AppInit -> Run group {groupId}");

            if(_isStarted)
            {
                Debug.Log($"AppInit -> Group {groupId} queued (group {_runningGroupId} still running)");
                _pendingGroups.Enqueue(groupId);
                return;
            }

            _isStarted = true;
            _runningGroupId = groupId;

            BuildSteps(groupId);

            if (_steps.Count == 0)
            {
                Done(null);
                return;
            }

            _runRoutine = _coroutineRunner.StartCoroutine(RunCoroutine());
        }

        public void Reset()
        {
            CleanupCurrentGroup();
            _pendingGroups.Clear();
        }

        public void Dispose()
        {
            Reset();
        }

        private void BuildSteps(int groupId)
        {
            Dictionary<Type, InitStepBase> unorderedSteps = _stepsProvider.GetSteps();

            Dictionary<Type, StepConfig> group = unorderedSteps
                .Select(o =>
                {
                    var attribute = o.Key.GetCustomAttribute<InitStepOrder>(false) ?? InitStepOrder.Default;
                    return new StepConfig(o.Key, o.Value, attribute);
                })
                .Where(sc => sc.Order.GroupId == groupId)
                .ToDictionary(sc => sc.Type, sc => sc);

            _steps = new List<InitStepBase>(group.Count);

            if (group.Count == 0)
            {
                Debug.LogWarning($"AppInit -> No steps registered for group {groupId}");
                return;
            }

            ProcessGroup(group);

            Debug.Log($"Init steps order (group {groupId}):\n {string.Join("\n ", _steps.Select(o => o.GetType().Name))}");
        }

        private void ProcessGroup(Dictionary<Type, StepConfig> group)
        {
            var sortHash = new HashSet<InitStepBase>();
            var edges = new HashSet<(InitStepBase, InitStepBase)>();

            foreach(var pair in group)
            {
                var dependencies = pair.Value.Order.Dependencies;

                for (var i = 0; i < dependencies.Count; i++)
                {
                    if (!group.TryGetValue(dependencies[i], out var dependency))
                    {
                        throw new Exception(
                            $"Init Step \"{pair.Key.Name}\" has dependency \"{dependencies[i].Name}\" which is not in group {pair.Value.Order.GroupId}");
                    }

                    var edge = (dependency.Step, pair.Value.Step);
                    edges.Add(edge);
                }

                sortHash.Add(pair.Value.Step);
            }

            var groupResult = sortHash.TopologicalSort(edges, (first, second) => first == second);
            _steps.AddRange(groupResult);
        }

        private IEnumerator RunCoroutine()
        {
            yield return null;

            foreach(var step in _steps)
            {
                var stepName = step.GetType().Name;
                Debug.Log($"Start init step: {stepName}");

                using (GetOpTimer(stepName))
                {
                    step.Start();

                    while(step.State == InitStepBase.StateCode.InProgress)
                    {
                        yield return null;
                    }
                }

                if(step.State == InitStepBase.StateCode.Error)
                {
                    Debug.LogError($"Init step failed: {stepName} / ex message: {step.Error.Message}" );
                    Done(step.Error);
                    yield break;
                }

                Debug.Log($"End init step: {stepName}");
            }

            Done(null);
        }

        private void Done(Exception exception)
        {
            var groupId = _runningGroupId;
            CleanupCurrentGroup();

            if(exception != null)
            {
                Debug.LogError($"Initialization is failed for group {groupId}! See above...");
            }
            else
            {
                Debug.Log($"Initialization is completed for group {groupId}!");
            }

            _signalBus.Fire(new InitGroupCompletedSignal(groupId));

            if (_pendingGroups.Count > 0)
            {
                Run(_pendingGroups.Dequeue());
            }
        }

        private void CleanupCurrentGroup()
        {
            if (_steps != null)
            {
                foreach(var step in _steps)
                {
                    step.Reset();
                }
            }

            if (_runRoutine != null && _coroutineRunner != null)
            {
                _coroutineRunner.StopCoroutine(_runRoutine);
                _runRoutine = null;
            }

            _steps = null;
            _isStarted = false;
        }

        private Timer GetOpTimer(string stepName)
        {
            return new Timer(stepName);
        }

        private class StepConfig
        {
            public readonly Type Type;
            public readonly InitStepBase Step;
            public readonly InitStepOrder Order;

            public StepConfig(Type type, InitStepBase step, InitStepOrder order)
            {
                Type = type;
                Step = step;
                Order = order;
            }
        }

        private readonly struct Timer : IDisposable
        {
            private readonly string _typeName;
            private readonly DateTime _startTime;

            public Timer(string typeName)
            {
                _typeName = typeName;
                _startTime = DateTime.Now;
            }

            public void Dispose()
            {
                Debug.Log($"Profile init step: {_typeName} {(DateTime.Now - _startTime).ToString()}");
            }
        }
    }
}
