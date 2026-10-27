using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.Strategies
{
    public class FireRateValidator : IFireRateValidator
    {
        public bool CanShoot(float lastShotTime, float fireRate)
        {
            var result = (Time.time - lastShotTime) >= fireRate;
            return result;
        }
    }
}