using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(10)]
    public class Module1 : IOrdered
    {
        public Module1()
        {
            
        }

        public void StartExecute()
        {
            Debug.Log("1");
        }
    }
}