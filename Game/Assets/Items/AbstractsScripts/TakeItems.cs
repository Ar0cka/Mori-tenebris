using System;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class TakeItems : MonoBehaviour
    {
        [Inject] private IInventoryAdder inventoryAdder;

        private ItemInstance _itemInstance;
        private int _countAdd;

        public void Initialize(int countItem, ItemScrObj itemScr)
        {
            _itemInstance = new ItemInstance(itemScr.GetItemData());
            _countAdd = countItem;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventoryAdder.AddItemToInventory(_itemInstance, _countAdd);
                }
            }
        }
    }
}