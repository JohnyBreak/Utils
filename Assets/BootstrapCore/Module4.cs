using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(SecondStage), 4)]
    public class Module4 : IExecutableModule
    {
        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("4");
            return Promise.Success();
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }
    }
}