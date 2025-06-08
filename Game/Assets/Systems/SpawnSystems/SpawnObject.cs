using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.SpawnMonsterSystem
{
    public class SpawnObject : MonoBehaviour
    {
        public List<GameObject> Spawn(PrefabSpawnSettings spawnSettings, int capacity)
        {
            List<GameObject> spawnObjects = new List<GameObject>();

            for (int i = 0; i < capacity; i++)
            {
                GameObject currentPrefab = Instantiate(spawnSettings.prefab, spawnSettings.parent);
                spawnObjects.Add(currentPrefab);
                currentPrefab.SetActive(false);
            }

            return spawnObjects;
        }

        public GameObject Spawn(PrefabSpawnSettings spawnSettings)
        {
            GameObject currentPrefab = Instantiate(spawnSettings.prefab, spawnSettings.parent);
            currentPrefab.SetActive(false);
            
            return currentPrefab;
        }
    }

    [Serializable]
    public class PrefabSpawnSettings
    {
        public GameObject prefab;
        public Transform parent;
    }
}