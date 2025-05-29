using System;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class TakeItems : MonoBehaviour
    {
        [SerializeField] private GameObject putText;
        [SerializeField] private ItemScrObj itemScrObj;

        private ItemInstance _itemInstance;
        private int _countAdd;

        private bool _initialized;

        public void Initialize(int countItem)
        {
            _itemInstance = new ItemInstance(itemScrObj.GetItemData());
            _countAdd = countItem;
            _initialized = true;
        }

        public PutItem TakeItem()
        {
            if (!_initialized) return null;
            
            Destroy(this.gameObject);
            
            return new PutItem(_itemInstance, 1);
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