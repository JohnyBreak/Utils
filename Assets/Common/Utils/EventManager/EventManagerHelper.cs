using System;

namespace Common.Utils.EventManager
{
    internal class EventManagerHelper
    {
        public static bool IsNull(object obj)
        { 
            return obj is UnityEngine.Object unityObject && unityObject == null;
        }
            
    }
}