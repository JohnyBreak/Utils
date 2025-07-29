using System;
using System.Collections.Generic;

namespace Common.Utils.EventManager
{
    public interface IEventManager
    {
        void Init();
        void Subscribe<T>(object listener, Action<Dictionary<Type, object>> action);
        void Subscribe<T>(object listener, Action action);
        void Subscribe<U, T>(object listener, Action<T> action);
        void UnSubscribe<T>(object listener);
        void TriggerEvent<T>(Dictionary<Type, object> message);
        void TriggerEvent<T>();
        void TriggerEvent<U, T>(T arg);
        void UnSubscribeNulls<U, T>() where T : class, IActionWrapper;
    }
}
