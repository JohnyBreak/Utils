using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Example
{
    public class AmmoComponent : IUseComponent
    {
        public int Ammo;
        public int MagSize;

        public bool CanUse()
        {
            return Ammo > 0;
        }

        public void Consume()
        {
            Ammo--;
            if (Ammo < 0)
            {
                Ammo = 0;                
            }
        }
    }
}