using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.WeaponSystem.Example.AttackComponents
{
    public class BulletMagazine : IItemComponent
    {
        private readonly BulletConfig _config;
        private readonly int _capacity;
        private int _current;
        
        public BulletMagazine(BulletConfig config, int capacity)
        {
            _config = config;
            _capacity = capacity;
            _current = _capacity;
        }
        
        public BulletConfig GetBulletConfig()
        {
            return _config;
        }
        
        public int GetCapacity()
        {
            return _capacity;
        }

        public void Fill(int bulletsAmount)
        {
            if (bulletsAmount <= 0)
            {
                return;
            }

            _current += bulletsAmount;

            if (_current > _capacity)
            {
                _current = _capacity;
            }
        }

        public void Consume()
        {
            if (_current <= 0)
            {
                return;
            }

            _current--;
            
            if (_current < 0)
            {
                _current = 0;
            }
        }

        public bool HasAmmo()
        {
            return _current > 0;
        }

        public int GetCurrentAmmo()
        {
            return _current;
        }
    }
}