using Common.Utils.Items.Runtime;

namespace Common.Utils.Items.Example
{
    public class Equipment
    {
        private IItem _current;
        
        public void Equip(IItem item)
        {
            _current = item;
        }

        public IItem GetCurrentWeapon()
        {
            return _current;
        }
    }
}