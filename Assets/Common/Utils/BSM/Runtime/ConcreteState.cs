using System;

namespace Common.BSM.Runtime
{
    public class ConcreteState : IState
    {
        private readonly Action _onEnter;
        private readonly Action _onUpdate;
        private readonly Action _onExit;

        public ConcreteState(
            Action onEnter,
            Action onUpdate,
            Action onExit)
        {
            _onEnter = onEnter;
            _onUpdate = onUpdate;
            _onExit = onExit;
        }

        public void Enter()
        {
            _onEnter?.Invoke();
        }

        public void Update()
        {
            _onUpdate?.Invoke();
        }

        public void Exit()
        {
            _onExit?.Invoke();
        }
    }
}