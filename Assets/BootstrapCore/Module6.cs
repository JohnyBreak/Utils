using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(ThirdStage), 6)]
    public class Module6 : IExecutableModule
    {
        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("6");
            return Promise.Success();
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }
    }
}