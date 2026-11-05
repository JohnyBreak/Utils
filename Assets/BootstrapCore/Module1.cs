using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(10)]
    public class Module1 : IExecutableModule
    {
        public Module1()
        {
            
        }

        public void Execute()
        {
            Debug.Log("1");
        }
    }
}