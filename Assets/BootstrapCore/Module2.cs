using Bootstrap;
using UnityEngine;
using Zenject;

namespace Summer
{
    [SummerSettings(typeof(MainStage), 2)]
    public class Module2 : MonoBehaviour, IExecutableModule
    {
        [Inject] private Module1 _module1;

        public Promise Execute()
        {
            Debug.Log("2");
            return Promise.Success();
        }
    }
}