using System.Collections;
using UnityEngine;

namespace Common.Utils.CoroutineProvider
{
    public class CoroutineProvider : MonoBehaviour, ICoroutineRunner
    {
        private static CoroutineProvider _self;
        
        public static Coroutine DoCoroutine(IEnumerator enumerator)
        {
            if (_self == null)
            {
                _self = new GameObject("CoroutineProvider").AddComponent<CoroutineProvider>();
            }

            return _self.StartCoroutine(enumerator);
        }

        public static void Stop(IEnumerator enumerator)
        {
            if (_self == null)
            {
                Debug.LogError("CoroutineProvider doesnt exist");
                return;
            }
            
            _self.StopCoroutine(enumerator);
        }
        
        public static void Stop(Coroutine coroutine)
        {
            if (_self == null)
            {
                Debug.LogError("CoroutineProvider doesnt exist");
                return;
            }
            
            _self.StopCoroutine(coroutine);
        }

        public static void Destroy()
        {
            if (_self == null)
            {
                return;
            }
            _self.StopAllCoroutines();
            Destroy(_self.gameObject);
            _self = null;
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