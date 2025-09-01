using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.CustomDictionary
{
    [Serializable]
    public class SerializableAttribute<TKey, TValue> : ISerializationCallbackReceiver
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();
        
        private Dictionary<TKey, TValue> _dictionary;
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in _dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _dictionary = new Dictionary<TKey, TValue>();

            for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
            {
                _dictionary.Add(keys[i], values[i]);
            }
        }

        void OnGUI()
        {
            foreach (KeyValuePair<TKey, TValue> pair in _dictionary)
                GUILayout.Label($"Key: {pair.Key}, Value: {pair.Value}");
        }
    }
}