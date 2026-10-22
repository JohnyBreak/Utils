using System.Collections.Generic;
using Common.Utils.BulletSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.Ammo;
using Common.Utils.WeaponSystem.Runtime.Magazine;

namespace Common.Utils.WeaponSystem.Example.Test.Pistol
{
    public class BulletMagazine : IMagazine
    {
        private readonly HashSet<BulletType> _acceptableBulletTypes;
        private IAmmo _ammo;
        private readonly int _capacity;
        private int _count;
        
        public BulletMagazine(
            HashSet<BulletType> acceptableBulletTypes,
            BulletAmmo ammo,
            int capacity)
        {
            _acceptableBulletTypes = acceptableBulletTypes;
            _ammo = ammo;
            _capacity= capacity;
        }

        public int GetCapacity()
        {
            return _capacity;
        }

        public void Consume()
        {
            if (!CanUse())
            {
                return;
            }

            _count--;
            EnsureAmmoAmount();
        }

        public bool CanUse()
        {
            return _count > 0;
        }

        public bool TryReload(IAmmo ammo)
        {
            if (!IsCorrectType(ammo, out var bulletAmmo))
            {
                return false;
            }

            if (!IsCurrentBulletType(bulletAmmo))
            {
                return false;
            }

            _count += bulletAmmo.GetAmount();
            EnsureAmmoAmount();

            return true;
        }

        public bool TrySetAmmo(IAmmo ammo)
        {
            if (!CanSet(ammo))
            {
                return false;
            }

            _ammo = ammo;
            return true;
        }

        public BulletConfig GetConfig()
        {
            return ((BulletAmmo)_ammo).GetConfig();
        }

        private bool CanSet(IAmmo ammo)
        {
            if (!IsCorrectType(ammo, out var bulletAmmo))
            {
                return false;
            }

            return _acceptableBulletTypes.Contains(bulletAmmo.GetConfig().Type);
        }

        private bool IsCorrectType(IAmmo ammo, out BulletAmmo bulletAmmo)
        {
            if (ammo is not BulletAmmo innerBulletAmmo)
            {
                bulletAmmo = default;
                return false;
            }

            bulletAmmo = innerBulletAmmo;

            return true;
        }

        private bool IsCurrentBulletType(BulletAmmo ammo)
        {
            return ((BulletAmmo)_ammo).GetConfig().Type == ammo.GetConfig().Type;
        }

        private void EnsureAmmoAmount()
        {
            if (_count < 0)
            {
                _count = 0;
                return;
            }
            
            if (_count > _capacity)
            {
                _count = _capacity;
            }
        }
    }
}