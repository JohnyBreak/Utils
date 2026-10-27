using Bootstrap;
using UnityEngine;

namespace Summer
{
    [SummerSettings(3)]
    public class Module3 : IOrdered
    {
        public Module3()
        {
            
        }

        public void StartExecute()
        {
            Debug.Log("3");
        }
    }
}