using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.BSM.Runtime
{
    public class BehaviourStateMachine : IDisposable
    {
        private readonly Dictionary<IState, List<Transition>> _localTransitions = new Dictionary<IState, List<Transition>>();
        private readonly List<Transition> _globalTransitions = new List<Transition>();

        private IState _currentState;

        public void Init(IState initialState)
        {
            _currentState = initialState;
            _currentState?.Enter();
        }
        
        public void Dispose()
        {
            Exit();
        }
        
        public void Exit()
        {
            _currentState?.Exit();
            _currentState = null;
        }
        
        public void Update()
        {
            try
            {
                if (TryChangeState())
                {
                    return;
                }

                UpdateCurrentState();
            }
            catch (BSMException e)
            {
                HandleException(e);
            }
        }
        
        public void AddTransition(IState from, Transition transition)
        {
            try
            {
                AddLocalTransition(from, transition);
            }
            catch (BSMException e)
            {
                HandleException(e);
            }
        }

        private void AddLocalTransition(IState from, Transition transition)
        {
            CheckStateValidity(from);
            CheckTransitionValidity(transition);
            CheckStatesIdentity(from, transition.To);

            if (!_localTransitions.ContainsKey(from))
            {
                _localTransitions[from] = new List<Transition>();
            }

            _localTransitions[from].Add(transition);
        }

        private void CheckStateValidity(IState state)
        {
            if (state == null)
            {
                ThrowException("State is null or invalid");
            }
        }
        
        private void CheckStatesIdentity(IState from, IState to)
        {
            if (from.Equals(to))
            {
                ThrowException($"States are equals | from: {from}, to: {to}");
            }
        }
        
        public void AddTransition(Transition transition)
        {
            try
            {
                AddGlobalTransition(transition);
            }
            catch (BSMException e)
            {
                HandleException(e);
            }
        }

        private void AddGlobalTransition(Transition transition)
        {
            CheckTransitionValidity(transition);
                
            _globalTransitions.Add(transition);
        }

        private void CheckTransitionValidity(Transition transition)
        {
            if (transition == null || !transition.IsValid())
            {
                ThrowException("Transition is null or invalid");
            }
        }

        private bool TryChangeState()
        {
            var transition = GetTriggeredTransition();
                
            if (transition.IsValid())
            {
                ChangeState(transition.To);
                return true;
            }

            return false;
        }

        private void ChangeState(IState toState)
        {
            _currentState?.Exit();
            _currentState = toState;
            _currentState?.Enter();
        }

        private Transition GetTriggeredTransition()
        {
            var transition = GetTriggeredTransitionFromList(_globalTransitions);

            if (_currentState != null && !transition.IsValid())
            {
                if (!_localTransitions.TryGetValue(_currentState, out var transitions))
                {
                    return Transition.GetEmpty();
                }

                transition = GetTriggeredTransitionFromList(transitions);
            }

            if (!transition.IsValid())
            {
                return Transition.GetEmpty();
            }

            return transition;
        }

        private Transition GetTriggeredTransitionFromList(List<Transition> transitions)
        {
            if (transitions == null)
            {
                ThrowException("List of transitions is null");
            }

            foreach (var transition in transitions)
            {
                if (transition.Condition.Invoke())
                {
                    return transition;
                }
            }
            
            return Transition.GetEmpty();
        }

        private void UpdateCurrentState()
        {
            _currentState?.Update();
        }

        private void ThrowException(string message)
        {
            throw new BSMException(message);
        }
        
        private void HandleException(BSMException e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }
}