using System;
using System.Collections.Generic;
using DefaultNamespace.Zenject;
using UnityEngine;
using Zenject;

namespace Systems.SpawnMonsterSystem
{
    public class SpawnMonster : MonoBehaviour
    {
        [SerializeField] private List<GameObject> monsterForLevel;
        [SerializeField] private List<Transform> spawnPoints;
        
        [Inject] private ISpawnSceneObject _spawnGameObject;

        public void Initialize()
        {
            for (int i = 0; i < monsterForLevel.Count; i++)
            {
                if (monsterForLevel[i] != null || spawnPoints[i] != null)
                {
                    Debug.Log("Creating monster");
                    var monster = _spawnGameObject.Create(monsterForLevel[i].gameObject);
                    monster.transform.position = spawnPoints[i].position;
                }
            }
        }
    }
}