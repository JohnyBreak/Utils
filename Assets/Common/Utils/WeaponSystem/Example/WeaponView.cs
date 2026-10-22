using Common.Utils.WeaponSystem.Runtime;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example
{
    public class WeaponView : MonoBehaviour, IWeaponView
    {
        [SerializeField] private Transform _shootPosition;
        
        public Vector3 GetShootPosition()
        {
            return _shootPosition.position;
        }
    }
}