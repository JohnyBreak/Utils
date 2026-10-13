using Common.Utils.Items.Runtime.Components;

namespace Common.Utils.Items.Example
{
    public class ShootComponent : IUseComponent
    {
        public int Amo;
        public int MagSize;

        public bool CanUse()
        {
            return Amo > 0;
        }

        public void Shoot()
        {
            Amo--;
            if (Amo < 0)
            {
                Amo = 0;                
            }
        }
    }
}