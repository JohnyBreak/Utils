using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(FirstStage), 1)]
    public class Module1 : IExecutableModule
    {
        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("1");
            return Promise.Success();
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }
    }
}