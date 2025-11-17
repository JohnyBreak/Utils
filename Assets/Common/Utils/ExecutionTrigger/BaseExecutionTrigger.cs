using UnityEngine;

namespace ExecutionTriggers
{
    public abstract class BaseExecutionTrigger : MonoBehaviour
    {
        [SerializeField] protected BaseTriggerExecutor[] _executors;
        
        private void OnTriggerEnter(Collider other)
        {
            if(Predicate(other) == false)
            {
                return;
            }

            OnEnter(other);
        }

        protected abstract bool Predicate(Collider other);
        protected abstract void OnEnter(Collider other);
    }
}