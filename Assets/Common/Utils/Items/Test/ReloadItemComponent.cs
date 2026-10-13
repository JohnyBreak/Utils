using Common.Utils.BulletSystem.Runtime;
using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Test
{
    public class ReloadItemComponent: IUseComponent
    {
        private readonly GunData _gunData;
        
        public ReloadItemComponent(GunData gunData)
        {
            _gunData = gunData;
        }

        public bool CanUse()
        {
            return _gunData.CanReload();
        }
        
        public void Reload(int amount)
        {
            _gunData.Reload( amount);
        }

        public int GetMagSize()
        {
            return _gunData.GetMagSize();
        }

        public BulletConfig GetBulletInfo()
        {
            return _gunData.GetBulletInfo();
        }
    }
}