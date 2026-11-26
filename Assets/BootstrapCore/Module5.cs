using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(ThirdStage), 5, new []{typeof(Module3)})]
    public class Module5 : IExecutableModule
    {
        public Promise PreExecute()
        {
            return Promise.Success();
        }

        public Promise Execute()
        {
            Debug.Log("5");
            return Promise.Success();
        }

        public Promise PostExecute()
        {
            return Promise.Success();
        }
    }
}