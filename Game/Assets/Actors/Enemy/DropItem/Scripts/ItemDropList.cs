using System;
using System.Collections.Generic;
using DefaultNamespace.Zenject;
using Enemy.Events;
using EventBusNamespace;
using Player.Inventory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Actors.Enemy.DropItem.Scripts
{
    public class ItemDropList : MonoBehaviour
    {
        [SerializeField] private List<GameObject> itemsReference;
        [SerializeField] private Transform spawnItemPosition;
        [SerializeField] private int maxDrop;

        [Inject] private ISpawnProjectObject _spawnProjectObject;
        
        private int _randomCountItemDrop;

        private void Awake()
        {
            EventBus.Subscribe<SendDieEventEnemy>(e => DropItem());
        }

        private void DropItem()
        {
            _randomCountItemDrop = Random.Range(0, maxDrop);
            
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
                float offset = Random.Range(-2f, 2f);
                
                GameObject itemPrefab = _spawnProjectObject.Create(drop);
                itemPrefab.transform.position = (Vector2)spawnItemPosition.position + new Vector2(0, offset);
                SpawnAnimation spawnAnimation = itemPrefab?.GetComponent<SpawnAnimation>();
                TakeItems takeItems = itemPrefab?.GetComponent<TakeItems>();

                if (spawnAnimation != null)
                {
                    takeItems.Initialize(1);
                    spawnAnimation.PlaySpawnAnimation();
                }
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<SendDieEventEnemy>(e => DropItem());
        }
    }
}