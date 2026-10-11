using UnityEngine;

namespace Common.Utils.Items.External
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] private Transform _shootPosition;
        
        public Transform GetShootPosition()
        {
            return _shootPosition;
        }
    }
}