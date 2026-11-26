using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(FirstStage), 2)]
    public class Module2 : IExecutableModule
    {
        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("2");
            return Promise.Success();
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }
    }
}