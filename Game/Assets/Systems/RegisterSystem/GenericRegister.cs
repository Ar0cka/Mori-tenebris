using System;
using System.Collections.Generic;
using ConsoleApp.Runtime;

namespace Systems.RegisterSystem
{
    public class GenericRegister<TType, TObject>
    {
        private Dictionary<TType, TObject> _objectDictionary = new Dictionary<TType, TObject>();

        public virtual void Register(TType objectType, TObject referenceObject)
        {
            if (_objectDictionary.TryAdd(objectType, referenceObject))
            {
                ConsoleLogger.Info("Added object to dictionary");
            }
            else
            {
                ConsoleLogger.Error($"In dictionary has already been added object: {typeof(TObject).Name}");
            }
        }

        public TObject GetObject(TType objectType)
        {
            if (_objectDictionary.TryGetValue(objectType, out TObject result))
            {
                ConsoleLogger.Info("Getting object from dictionary");
                return result;
            }
            
            ConsoleLogger.Error("Getting object from dictionary failed");
            return default(TObject);
        }
    }
}