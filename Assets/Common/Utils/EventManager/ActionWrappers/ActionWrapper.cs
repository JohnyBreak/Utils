using System;
using System.Collections.Generic;

namespace Common.Utils.EventManager
{
    public class ActionWrapper : IActionWrapper
    {
        public event Action<Dictionary<Type, object>> Action; 
        public object Invoker { get; set; }
        
        public ActionWrapper(object invoker, Action<Dictionary<Type, object>> action)
        {
            Invoker = invoker;
            Action = action;
        }

        public bool Invoke(Type eventName, Dictionary<Type, object> data)
        {
            if (Invoker == null || EventManagerHelper.IsNull(Invoker))
            {
                return false;
            }

            Action?.Invoke(data);
            return true;
        }
    }
}