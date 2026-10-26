using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime.Helpers
{
    public class ShootPositionProvider : IshootPositionProvider
    {
        private readonly IWeaponView _view;

        public ShootPositionProvider(IWeaponView view)
        {
            _view = view;
        }

        public Vector3 Get()
        {
            return _view.GetShootPosition();
        }
    }
}