using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils.EventManager
{
    public class EventManager : IEventManager
    {
        private static EventManager _eventManager;

        private Dictionary<Type, Dictionary<object, object>> _events;
        
        public void Init()
        {
            if (_events == null)
            {
                _events = new();
            }

            _eventManager = this;
        }
        
        public static void Listen<T>(object listener, Action<Dictionary<Type, object>> action)
        {
            if (_eventManager == null)
            {
                Debug.LogError("EventManager >>> is null");
                return;
            }
            
            _eventManager.Subscribe<T>(listener, action);
        }
        
        public void Subscribe<T>(object listener, Action<Dictionary<Type, object>> action)
        {
            if (SubscribeCommon<T>(listener))
            {
                _events[typeof(T)].Add(listener, new ActionWrapper(listener, action));
            }
        }

        public static void Listen<T>(object listener, Action action)
        {
            if (_eventManager == null)
            {
                Debug.LogError("EventManager >>> is null");
                return;
            }
            
            _eventManager.Subscribe<T>(listener, action);
        }
        
        public void Subscribe<T>(object listener, Action action)
        {
            if (SubscribeCommon<T>(listener))
            {
                _events[typeof(T)].Add(listener, new ActionWrapperWithZeroArgs(listener, action));
            }
        }
        
        public static void Listen<U, T>(object listener, Action<T> action)
        {
            if (_eventManager == null)
            {
                Debug.LogError("EventManager >>> is null");
                return;
            }
            
            _eventManager.Subscribe<U, T>(listener, action);
        }
        
        public void Subscribe<U, T>(object listener, Action<T> action)
        {
            if (SubscribeCommon<U>(listener))
            {
                _events[typeof(U)].Add(listener, new ActionWrapperWithOneArgs<T>(listener, action));
            }
        }

        public static void StopListen<T>(object listener)
        {
            if (_eventManager == null)
            {
                Debug.LogError("EventManager >>> is null");
                return;
            }
            
            _eventManager.UnSubscribe<T>(listener);
        }
        
        public void UnSubscribe<T>(object listener)
        {
            var type = typeof(T);

            if (_events.ContainsKey(type))
            {
                _events[type].Remove(listener);
            }
        }
        
        public static void Trigger<T>(Dictionary<Type, object> message)
        {
            _eventManager.TriggerEvent<T>(message);
        }
        
        public void TriggerEvent<T>(Dictionary<Type, object> message)
        {
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            bool allNotNulls = true;
            foreach (var actionWrapper in events.Values)
            {
                if (actionWrapper is not ActionWrapper wrapper)
                {
                    Debug.LogError($" when you trigger {type} you passed illegal number of arguments");
                    continue;
                }

                allNotNulls = allNotNulls && wrapper.Invoke(type, message);
            }

            if (!allNotNulls)
            {
                _eventManager.UnSubscribeNulls<T, ActionWrapper>();
            }
        }
        
        public static void Trigger<T>()
        {
            _eventManager.TriggerEvent<T>();
        }
        
        public void TriggerEvent<T>()
        {
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            bool allNotNulls = true;
            foreach (var actionWrapper in events.Values)
            {
                if (actionWrapper is not ActionWrapperWithZeroArgs wrapper)
                {
                    Debug.LogError($" when you trigger {type} you passed illegal number of arguments");
                    continue;
                }

                allNotNulls = allNotNulls && wrapper.Invoke(type);
            }

            if (!allNotNulls)
            {
                _eventManager.UnSubscribeNulls<T, ActionWrapperWithZeroArgs>();
            }
        }
        
        public static void Trigger<U, T>(T arg)
        {
            _eventManager.TriggerEvent<U, T>(arg);
        }
        
        public void TriggerEvent<U, T>(T arg)
        {
            var type = typeof(U);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            bool allNotNulls = true;
            foreach (var actionWrapper in events.Values)
            {
                if (actionWrapper is not ActionWrapperWithOneArgs<T> wrapper)
                {
                    Debug.LogError($"When you trigger {type} you passed illegal number of arguments");
                    continue;
                }

                allNotNulls = allNotNulls && wrapper.Invoke(type, arg);
            }

            if (!allNotNulls)
            {
                _eventManager.UnSubscribeNulls<U, ActionWrapperWithOneArgs<T>>();
            }
        }

        public void UnSubscribeNulls<U, T>() where T : class, IActionWrapper
        {
            var type = typeof(U);
            if (!_events.ContainsKey(type))
            {
                return;
            }
            
            Debug.LogError($"you haven't unsubscribed destroyed object in {type}. I unsubscribed it for you");

            List<object> toRemove = new();
            foreach (var actionWrapper in _events[type].Values)
            {
                if (actionWrapper is not T wrapperT)
                {
                    Debug.LogError($"When you trigger {type} you passed illegal number of arguments");
                    continue;
                }

                if (EventManagerHelper.IsNull(wrapperT.Invoker))
                {
                    toRemove.Add(wrapperT.Invoker);
                }
            }

            foreach (var obj in toRemove)
            {
                _events[type].Remove(obj);
            }
        }

        private bool SubscribeCommon<T>(object listener)
        {
            var type = typeof(T);
            if (!_events.ContainsKey(type))
            {
                _events.Add(type, new Dictionary<object, object>());
            }

            if (_events[type].ContainsKey(listener))
            {
                Debug.LogError($"you try to subscribe on {type} twice with object: {listener} type: {listener.GetType()}");
                return false;
            }

            return true;
        }
    }
}