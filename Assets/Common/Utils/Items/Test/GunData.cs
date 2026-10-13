using Common.Utils.BulletSystem.Runtime;
using UnityEngine;

namespace Common.Utils.Items.Test
{
    public class GunData
    {
        private readonly int _magSize;
        private readonly GunView _view;
        private readonly BulletConfig _bulletDef;
        private int _amo;
        
        public GunData(
            int magSize, 
            GunView view, 
            BulletConfig bulletDef)
        {
            _magSize = magSize;
            _view = view;
            _bulletDef = bulletDef;
            _amo = _magSize;
        }

        public bool CanReload()
        {
            return _amo < _magSize;
        }

        public void Reload()
        {
            _amo = _magSize;
            Debug.LogError($"reloaded {_amo}");
        }
        
        public void Reload(int amount)
        {
            if (amount > _magSize)
            {
                Reload(_magSize);
                return;
            }

            _amo = amount;
            
            Debug.LogError($"reloaded {_amo}");
        }
        
        public bool CanShoot()
        {
            return _amo > 0;
        }

        public void Shoot()
        {
            _amo--;
            if (_amo < 0)
            {
                _amo = 0;                
            }
            Debug.LogError(_amo);
        }

        public Transform GetShootPosition()
        {
            return _view.GetShootPosition();
        }

        public int GetMagSize()
        {
            return _magSize;
        }

        public BulletConfig GetBulletInfo()
        {
            return _bulletDef;
        }
    }
}