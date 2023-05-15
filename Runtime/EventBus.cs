using System;
using System.Collections.Generic;
using Ostium11.Extensions;
using UnityEngine;

namespace Ostium11
{
    public static class EventBus
    {
        static readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Clear() => _handlers.Clear();

        public static void Subscribe<T>(Action<T> handler) => _handlers.GetOrCreate(typeof(T)).Add(handler);

        public static void Subscribe<T>(Action handler) => _handlers.GetOrCreate(typeof(T)).Add(handler);

        public static void Publish<T>() where T : new() => Publish(new T());

        public static void Publish<T>(T message)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    if (handler is Action<T> action)
                        action(message);
                    else
                        ((Action)handler)();
                }
            }
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
                handlers.Remove(handler);
        }

        public static void Unsubscribe<T>(Action handler)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
                handlers.Remove(handler);
        }
    }
}