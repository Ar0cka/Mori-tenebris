using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventBusNamespace
{
    public class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        public static void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var existing))
                _events[type] = Delegate.Combine(existing, action);
            else
                _events[type] = action;
        }

        public static void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var existing))
            {
                var current = Delegate.Remove(existing, action);
                if (current == null) _events.Remove(type);
                else _events[type] = current;
            }
        }

        public static void Publish<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out var del))
            {
                if (del is Action<T> action)
                    action.Invoke(eventData);
            }
        }

        public static void ClearAll() => _events.Clear();
    }
}