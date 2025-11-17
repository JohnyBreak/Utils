using UnityEngine;

namespace ExecutionTriggers
{
    public class LayerPredicate : BaseExecutionPredicate
    {
        [SerializeField] protected LayerMask _mask;

        public override bool Predicate(Collider other)
        {
            return ((1 << other.gameObject.layer) & _mask) != 0;
        }
    }
}