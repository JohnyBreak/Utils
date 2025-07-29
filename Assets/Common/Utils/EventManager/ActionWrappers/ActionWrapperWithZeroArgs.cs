using System;

namespace Common.Utils.EventManager
{
    public class ActionWrapperWithZeroArgs : IActionWrapper
    {
        public event Action Action; 
        public object Invoker { get; set; }
        
        public ActionWrapperWithZeroArgs(object invoker, Action action)
        {
            Invoker = invoker;
            Action = action;
        }

        public bool Invoke(Type eventName)
        {
            if (Invoker == null || EventManagerHelper.IsNull(Invoker))
            {
                return false;
            }

            Action?.Invoke();
            return true;
        }
    }
}