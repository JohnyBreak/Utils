namespace Common.Utils.WeaponSystem.Runtime.Components
{
    public class ShotgunShootComponent : IShootComponent
    {
        private readonly int _bulletsPerShot;
        private readonly float _fireRate;
        private readonly float _spreadAngle;

        public ShotgunShootComponent(
            int bulletsPerShot, 
            float fireRate,
            float spreadAngle)
        {
            _bulletsPerShot = bulletsPerShot;
            _fireRate = fireRate;
            _spreadAngle = spreadAngle;
        }

        public float GetFireRate()
        {
            return _fireRate;
        }
        
        public int GetBulletsPerShot()
        {
            return _bulletsPerShot;
        }

        public float GetSpreadAngle()
        {
            return _spreadAngle;
        }
    }
}