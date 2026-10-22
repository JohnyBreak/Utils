using Common.Utils.BulletSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.Ammo;

namespace Common.Utils.WeaponSystem.Example.Test.Pistol
{
    public readonly struct BulletAmmo : IAmmo
    {
        private readonly BulletConfig _config;
        private readonly int _amount;

        public BulletAmmo(BulletConfig config, int amount)
        {
            _config= config;
            _amount = amount;
        }

        public int GetAmount()
        {
            return _amount;
        }
        
        public BulletConfig GetConfig()
        {
            return _config;
        }
    }
}