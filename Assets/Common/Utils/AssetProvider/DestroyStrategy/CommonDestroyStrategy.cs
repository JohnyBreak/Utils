using UnityEngine;

namespace AssetProvider.DestroyStrategy
{
    public class CommonDestroyStrategy : IDestroyStrategy
    {
        public void Destroy(GameObject gameObject)
        {
            gameObject.SetActive(false);
            Object.Destroy(gameObject);
        }

        public void Destroy(Component component)
        {
            var gameObject = component.GetComponent<GameObject>();
            if (gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}