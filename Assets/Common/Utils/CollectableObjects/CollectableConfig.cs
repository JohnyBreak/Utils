namespace Collectables
{
    public abstract class CollectableConfig
    {
        public abstract int GetCollectableType();
    }
    
    public class PistolAmmoConfig : CollectableConfig
    {
        public int Amount;
        
        public override int GetCollectableType()
        {
            return CollectablesTypes.PistolAmmo;
        }
    }

    public class MedkitConfig : CollectableConfig
    {
        public int HealAmount;
        
        public override int GetCollectableType()
        {
            return CollectablesTypes.Health;
        }
    }

    public class KeycardConfig : CollectableConfig
    {
        public int Level;
        
        public override int GetCollectableType()
        {
            return CollectablesTypes.Keycard;
        }
    }
}