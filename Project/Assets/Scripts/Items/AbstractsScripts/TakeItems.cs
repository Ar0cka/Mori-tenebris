using System;
using DG.Tweening;
using Enemy;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class TakeItems : MonoBehaviour
    {
        [SerializeField] private GameObject putText;

        private SpawnAnimation _spawnAnimation;
        
        private ItemInstance _itemInstance;
        private int _countAdd;

        private bool _initialized;

        public void Initialize(ItemData itemData, int countItem, SpawnAnimation spawnAnimation)
        {
            _spawnAnimation = spawnAnimation;
            _itemInstance = new ItemInstance(itemData);
            _countAdd = countItem;
            _initialized = true;
        }

        public PutItem TakeItem()
        {
            if (!_initialized) return null;
            
            Destroy(gameObject);
            
            return new PutItem(_itemInstance, _countAdd);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_initialized) return;

            if (other.CompareTag("Player"))
            {
                putText.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_initialized) return;

            if (other.CompareTag("Player"))
            {
                putText.SetActive(false);
            }
        }
    }

    public class PutItem
    {
        public ItemInstance ItemInstance;
        public int Count;
        public PutItem(ItemInstance itemInstance, int count)
        {
            ItemInstance = itemInstance;
            Count = count;
        }
    }
}