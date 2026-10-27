using Bootstrap;
using UnityEngine;
using Zenject;

namespace Summer
{
    [SummerSettings(2)]
    public class Module2 : MonoBehaviour, IOrdered
    {
        [Inject] private Module1 _module1;

        public void StartExecute()
        {
            Debug.Log("2");
        }
    }
}