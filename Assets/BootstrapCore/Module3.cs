using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(MainStage), 3)]
    public class Module3 : IExecutableModule
    {
        public Promise Execute()
        {
            Debug.Log("3");
            return Promise.Success();
        }
    }
}