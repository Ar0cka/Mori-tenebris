using System;
using System.Collections.Generic;
using Enemy.Events;
using EventBusNamespace;
using Player.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Enemy.DropItem.Scripts
{
    public class ItemDropList : MonoBehaviour
    {
        [SerializeField] private List<GameObject> itemsReference;
        [SerializeField] private Transform spawnItemPosition;
        [SerializeField] private int maxDrop;

        private int _randomCountItemDrop;

        private void Awake()
        {
            EventBus.Subscribe<SendDieEventEnemy>(e => DropItem());
        }

        private void DropItem()
        {
            Debug.Log("DropItem");
            
            _randomCountItemDrop = Random.Range(0, maxDrop);
            
            Debug.Log($"random count {_randomCountItemDrop}");

            if (_randomCountItemDrop == 0) return;
            
            List<GameObject> itemsForSpawn = new List<GameObject>();

            for (int i = 0; i < _randomCountItemDrop; i++)
            {
                itemsForSpawn.Add(itemsReference[Random.Range(0, itemsReference.Count)]);
            }

            if (itemsForSpawn.Count != 0)
            {
                Debug.Log($"Spawning {itemsForSpawn.Count}");
                Spawn(itemsForSpawn);
            }
        }

        private void Spawn(List<GameObject> dropList)
        {
            foreach (var drop in dropList)
            {
                float offset = Random.Range(1, -1);
                
                GameObject itemPrefab = Instantiate(drop);
                itemPrefab.transform.position = (Vector2)spawnItemPosition.position + new Vector2(0, offset);
                SpawnAnimation spawnAnimation = itemPrefab?.GetComponent<SpawnAnimation>();

                if (spawnAnimation != null)
                {
                    spawnAnimation.SpawnAnimationStarted(itemPrefab);
                }
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<SendDieEventEnemy>(e => DropItem());
        }
    }
}