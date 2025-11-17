using UnityEngine;

namespace ExecutionTriggers
{
    public abstract class BaseExecutionPredicate : MonoBehaviour
    {
        public abstract bool Predicate(Collider other);
    }
}