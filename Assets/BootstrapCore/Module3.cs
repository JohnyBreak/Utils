using System.Collections;
using Bootstrap;
using UnityEngine;
using Zenject;

namespace Summer
{
    [SummerSettings(typeof(SecondStage), 3)]
    public class Module3 : MonoBehaviour, IExecutableModule
    {
        [Inject] private Module1 _module1;
        private Promise _promise;

        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("3");
            
            _promise = new Promise();
            StartCoroutine(Delay());
            //return Promise.Success();
            return _promise;
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(5);
            
            Debug.Log("3 complete");
            _promise.Complete(SummerStatus.Success);
        }
    }
}