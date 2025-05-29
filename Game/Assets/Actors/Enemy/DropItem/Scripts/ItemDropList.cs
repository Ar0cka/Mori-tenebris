using System;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace.Zenject;
using Enemy;
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
        [SerializeField] private List<ItemScrObj> itemsReference;
        [SerializeField] private Transform spawnItemPosition;
        [SerializeField] private int maxDrop;
        [SerializeField] private int minDrop;

        private int _randomCountItemDrop;
        private Action<SendDieEventEnemy> _onDie;

        private void Awake()
        {
            _onDie = e => DropItem();
            EventBus.Subscribe(_onDie);
        }

        private void DropItem()
        {
            _randomCountItemDrop = Random.Range(minDrop, maxDrop);

            if (_randomCountItemDrop == 0 || itemsReference.Count == 0) return;

            Dictionary<ItemScrObj, int> items = new Dictionary<ItemScrObj, int>();

            for (int i = 0; i < _randomCountItemDrop; i++)
            {
                var item = itemsReference[Random.Range(0, itemsReference.Count)];

                if (items.ContainsKey(item))
                {
                    items[item]++;
                }
                else
                {
                    items.Add(item, 1);
                }
            }

            if (items.Count == 0) return;

            Spawn(items);
        }

        private void Spawn(Dictionary<ItemScrObj, int> dropList)
        {
            foreach (var drop in dropList)
            {
                int remainingCount = drop.Value;
                
                Debug.Log("Items for spawn count = " + remainingCount);

                while (remainingCount > 0)
                {
                    remainingCount = SpawnItems(drop.Key, remainingCount);
                }
            }
        }

        private int SpawnItems(ItemScrObj item, int itemCount)
        {
            ItemData itemData = item.GetItemData();

            int stackCount = Mathf.Min(itemData.maxStackInSlot, itemCount);

            float offset = Random.Range(-2f, 2f);

            GameObject itemPrefab = Instantiate(item.SpawnPrefab);

            itemPrefab.name = Guid.NewGuid().ToString("N");
            itemPrefab.transform.position = (Vector2)spawnItemPosition.position + new Vector2(0, offset);

            SpawnAnimation spawnAnimation = itemPrefab.GetComponent<SpawnAnimation>();
            TakeItems takeItems = itemPrefab.GetComponent<TakeItems>();

            if (spawnAnimation != null && takeItems != null)
            {
                takeItems.Initialize(itemData, stackCount);
                spawnAnimation.PlaySpawnAnimation();
            }
            else
            {
                Destroy(itemPrefab);
            }

            return itemCount - stackCount;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(_onDie);
        }
    }
}