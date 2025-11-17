using UnityEngine;

namespace ExecutionTriggers
{
    public class ExecutionTrigger : BaseExecutionTrigger
    {
        [SerializeField] private BaseExecutionPredicate _predicate;
        protected override bool Predicate(Collider other)
        { 
            return (_predicate != null) && _predicate.Predicate(other);
        }

        protected override void OnEnter(Collider other)
        {
            foreach (var executor in _executors)
            {
                executor.Execute();
            }
        }
    }
}