using System.Collections;
using UnityEngine;

namespace Common.Utils.CoroutineProvider
{
    public class GlobalCoroutineProvider : MonoBehaviour, ICoroutineRunner
    {
        private static GlobalCoroutineProvider _self;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static GlobalCoroutineProvider Get()
        {
            return _self;
        }

        public static Coroutine DoCoroutine(IEnumerator enumerator)
        {
            if (_self == null)
            {
                _self = new GameObject("GlobalCoroutineProvider").AddComponent<GlobalCoroutineProvider>();
            }

            return _self.StartCoroutine(enumerator);
        }

        public static void Stop(IEnumerator enumerator)
        {
            if (_self == null)
            {
                Debug.LogError("GlobalCoroutineProvider doesnt exist");
                return;
            }
            
            _self.StopCoroutine(enumerator);
        }
        
        public static void Stop(Coroutine coroutine)
        {
            if (_self == null)
            {
                Debug.LogError("GlobalCoroutineProvider doesnt exist");
                return;
            }
            
            _self.StopCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            if (_self == null)
            {
                return;
            }
            
            _self.StopAllCoroutines();
        }
    }
}
