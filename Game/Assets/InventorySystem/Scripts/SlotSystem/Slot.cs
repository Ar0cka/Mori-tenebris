using Enemy;
using Player.Inventory;
using Service;
using UnityEngine;

namespace SlotSystem
{
    public class Slot
    {
        private ItemInstance _currentItem;
        private int _currentCountItem;

        public bool IsFull => _currentItem != null && _currentCountItem == _currentItem.maxStack;
        public bool IsOccupied => _currentItem != null;

        public void CreateNewItem(ItemInstance itemInstance)
        {
            _currentItem = itemInstance;
        }

        public int AddNewItem(ItemInstance itemInstance, int amountItems)
        {
            if (_currentItem == null || amountItems <= 0)
            {
                Debug.LogError("Current item = " + _currentItem + " " + amountItems);
                return amountItems;
            }

            if (_currentItem.itemData.typeID != itemInstance.itemData.typeID) return amountItems; // сравнение по ID
            if (IsFull) return amountItems;

            int space = _currentItem.maxStack - _currentCountItem;
            int itemAddCount = Mathf.Min(amountItems, space);

            _currentCountItem += itemAddCount;
            _currentItem.amount = _currentCountItem;

            return amountItems - itemAddCount;
        }

        public int RemoveItem(ItemInstance itemInstance, int amountItems)
        {
            if (itemInstance == null || amountItems <= 0) return amountItems;

            if (!CheckItemInSlot(itemInstance.itemID)) return amountItems;

            int itemRemove = Mathf.Min(amountItems, _currentCountItem);
            _currentCountItem -= itemRemove;
            _currentItem.amount = _currentCountItem;
            
            if (_currentCountItem == 0)
            {
                ClearSlot();
            }
            
            return amountItems - itemRemove;
        }

        public bool CanCreate() => !IsOccupied && _currentItem == null;
        public int CurrentItemCount() => _currentCountItem;
        public bool CheckItemInSlot(string id) => _currentItem != null && _currentItem.itemID == id;
        public bool CheckItemInSlot(int id) => _currentItem != null && _currentItem.itemData.typeID == id;

        public ItemInstance UnEquipData()
        {
            var item = _currentItem;
            ClearSlot();
            return item;
        }
        
        public ItemInstance GetItemInstance() => _currentItem;
        
        private void ClearSlot()
        {
            _currentCountItem = 0;
            _currentItem = null;
        }
    }
}