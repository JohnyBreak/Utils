using Bootstrap;
using UnityEngine;
using Zenject;

namespace Summer
{
    [SummerSettings(2)]
    public class Module2 : MonoBehaviour, IExecutableModule
    {
        [Inject] private Module1 _module1;

        public void Execute()
        {
            Debug.Log("2");
        }
    }
}