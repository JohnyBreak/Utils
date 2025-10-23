using UnityEngine;

namespace AssetProvider.DestroyStrategy
{
    public interface IDestroyStrategy
    {
        void Destroy(GameObject gameObject);
        void Destroy(Component component);
    }
}