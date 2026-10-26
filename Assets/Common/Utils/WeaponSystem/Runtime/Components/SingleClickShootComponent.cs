namespace Common.Utils.WeaponSystem.Runtime.Components
{
    public class SingleClickShootComponent : IShootComponent
    {
        private readonly float _fireRate;

        public SingleClickShootComponent(float fireRate)
        {
            _fireRate = fireRate;
        }

        public float GetFireRate()
        {
            return _fireRate;
        }
    }
}