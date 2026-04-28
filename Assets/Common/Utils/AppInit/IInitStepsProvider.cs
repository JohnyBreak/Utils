using System;
using System.Collections.Generic;

namespace AppInit.InitSteps
{
    public interface IInitStepsProvider
    {
        Dictionary<Type, InitStepBase> GetSteps();
        public void Register(InitStepBase step);
        public void Unregister(InitStepBase step);
    }
}