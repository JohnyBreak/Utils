using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(typeof(MainStage), 3)]
    public class Module3 : IExecutableModule
    {
        public Module3()
        {
            
        }

        public void Execute()
        {
            Debug.Log("3");
        }
    }
}