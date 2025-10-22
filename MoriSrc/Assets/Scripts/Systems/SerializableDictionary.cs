using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Systems
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] protected List<TKey> keys = new List<TKey>();
        [SerializeField] protected List<TValue> values = new List<TValue>();
        
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            if (Dictionary == null)
            {
                Dictionary = new Dictionary<TKey, TValue>();
                
                for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
                {
                    if (!Dictionary.TryAdd(keys[i], values[i]))
                    {
                        Debug.LogError($"Dictionary contains duplicate keys {keys[i]}");
                    }
                }
            }
            
            return Dictionary;
        }
    }
}