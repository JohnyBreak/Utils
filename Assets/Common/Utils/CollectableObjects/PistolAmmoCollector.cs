namespace Collectables
{
    public class PistolAmmoCollector : ICollector
    {
        private readonly TestCollectableContainer _container;

        public PistolAmmoCollector(TestCollectableContainer container)
        {
            _container = container;
        }

        public bool TryCollect(CollectableConfig collectable)
        {
            if (collectable is PistolAmmoConfig ammo)
            {
                return TryCollect(ammo);
            }

            return false;
        }

        public int GetCollectableType()
        {
            return CollectablesTypes.PistolAmmo;
        }

        private bool TryCollect(PistolAmmoConfig pistolAmmo)
        {
            _container.PistolAmmo += pistolAmmo.Amount;
            return true;
        }
    }
}

