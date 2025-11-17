using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogError($"[ServiceLocator] service with type {typeof(T)} already registered");
                return;
            }

            _services.Add(type, service);
        }

        public static void Unregister<T>()
        {
            var type = typeof(T);
            _services.Remove(type);
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            throw new Exception($"Service of type {type.Name} is not registered in ServiceLocator.");
        }

        public static bool IsRegistered<T>()
        {
            return _services.ContainsKey(typeof(T));
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}
