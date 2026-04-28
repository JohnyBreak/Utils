using System;
using System.Collections.Generic;

namespace AppInit.InitSteps
{
    public class InitStepProvider : IInitStepsProvider
    {
        private Dictionary<Type, InitStepBase> _steps = new();

        public void Register(InitStepBase step)
        {
            _steps[step.GetType()] = step;
        }

        public void Unregister(InitStepBase step)
        {
            _steps.Remove(step.GetType());
        }

        public Dictionary<Type, InitStepBase> GetSteps()
        {
            return _steps;
        }
    }
}