using System;

namespace Common.Utils.EventManager
{
    public class ActionWrapperWithOneArgs<T> : IActionWrapper
    {
        public event Action<T> Action; 
        public object Invoker { get; set; }

        public ActionWrapperWithOneArgs(object invoker, Action<T> action)
        {
            Invoker = invoker;
            Action = action;
        }

        public bool Invoke(Type eventName, T arg1)
        {
            if (Invoker == null || EventManagerHelper.IsNull(Invoker))
            {
                return false;
            }
            
            Action?.Invoke(arg1);
            return true;
        }
    }
}