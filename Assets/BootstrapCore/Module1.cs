using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(MainStage), 10)]
    public class Module1 : IExecutableModule
    {
        public Promise Execute()
        {
            Debug.Log("1");
            return Promise.Success();
        }
    }
}