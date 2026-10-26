namespace Common.Utils.WeaponSystem.Runtime.Components
{
    public class HoldShootComponent : IShootComponent
    {
        private readonly float _fireRate;

        public HoldShootComponent(float fireRate)
        {
            _fireRate = fireRate;
        }

        public float GetFireRate()
        {
            return _fireRate;
        }
    }
}