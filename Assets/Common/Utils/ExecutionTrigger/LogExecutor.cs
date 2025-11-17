using UnityEngine;

namespace ExecutionTriggers
{
    public class LogExecutor : BaseTriggerExecutor
    {
        public override void Execute()
        {
            Debug.LogWarning("Execute Success");
        }
    }
}