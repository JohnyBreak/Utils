using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.Helpers
{
    public class AimDirection : IAimDirection
    {
        private readonly Transform _cameraTransform;

        public AimDirection(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }
        
        public Vector3 GetDirection()
        {
            if (!_cameraTransform)
            {
                return Vector3.zero;
            }

            return _cameraTransform.forward;
        }
    }
}