using System;
using System.Collections.Generic;
using Systems.SpawnMonsterSystem;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class ObjectPool
    {
        private PrefabSpawnSettings _spawnSettings;
        private SpawnObject _spawnObject;
        private List<GameObject> _pool;

        public ObjectPool(PrefabSpawnSettings spawnSettings, SpawnObject spawnObject, int startPoolCapacity)
        {
            if (spawnObject == null || spawnSettings == null)
            {
                #if UNITY_EDITOR 
                if (spawnObject == null)
                {
                    Debug.LogWarning("Spawn Object is null.");
                }

                if (spawnSettings == null)
                {
                    Debug.LogWarning("Spawn Settings is null.");
                }
                #endif // Logs
                return;
            }
            
            _spawnSettings = spawnSettings;
            _spawnObject = spawnObject;
            _pool = spawnObject.Spawn(_spawnSettings, startPoolCapacity);
        }

        public GameObject Get()
        {
            foreach (var item in _pool)
            {
                if (!item.activeInHierarchy)
                {
                    return item;
                }
            }
            
            var prefab = _spawnObject.Spawn(_spawnSettings);
            
            if (prefab != null) 
                _pool.Add(prefab);
            
            return prefab;
        }

        public void ReturnItemToPool(GameObject item)
        {
            item.SetActive(false);
        }
    }
}