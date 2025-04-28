using System;
using System.Collections.Generic;

namespace EventBusNamespace
{
    public class EventBus
    {
        private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        public static void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!_events.ContainsKey(type))
            {
                _events.Add(type, null);
                _events[type] = Delegate.Combine(_events[type], action);
            }
        }
        
        public static void Subscribe<T>(List<Action<T>> actions)
        {
            var type = typeof(T);
            
            foreach (var action in actions)
            {
                if (action == null) continue;
                
                if (!_events.ContainsKey(type))
                {
                    _events.Add(type, null);
                    _events[type] = Delegate.Combine(_events[type], action);
                }
            }
        }

        public static void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var existing))
            {
                _events[type] = Delegate.Remove(existing, action);
            }
        }
        
        public static void Unsubscribe<T>(List<Action<T>> actions)
        {
            var type = typeof(T);
            foreach (var action in actions)
            {
                if (action == null) continue;
                
                if (_events.TryGetValue(type, out var existing))
                {
                    _events[type] = Delegate.Remove(existing, action);
                }
            }
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var value))
            {
                (value as Action<T>)?.Invoke(eventData);
            }
        }

        
    }
}