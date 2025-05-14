using Enemy;
using UnityEngine;

namespace SlotSystem
{
    public class Slot
    {
        private ItemData _currentItem;
        private int _currentCountItem;
        public bool IsFull => _currentItem != null && _currentCountItem == _currentItem.maxStackInSlot;
        public bool IsOccupied => _currentItem != null;

        public void CreateNewItem(ItemData itemData)
        {
            _currentItem = itemData;
        }

        public int AddNewItem(ItemData itemData, int amountItems)
        {
            if (_currentItem == null || amountItems <= 0)
            {
                Debug.LogError("Current item = " + _currentItem + " " + amountItems);
                return amountItems;
            }

            if (IsFull) return amountItems;
            
            int space = itemData.maxStackInSlot - _currentCountItem;
            int itemAddCount = Mathf.Min(amountItems, space);
                
            _currentCountItem += itemAddCount;

            return amountItems - itemAddCount;
        }

        public int RemoveItem(ItemData itemData, int amountItems)   
        {
            if (itemData == null && amountItems <= 0) return amountItems;

            if (!CheckItemInSlot(itemData?.nameItem)) return amountItems;
            
            int itemRemove = Mathf.Min(amountItems, _currentCountItem);

            _currentCountItem -= itemRemove;

            if (_currentCountItem == 0)
            {
                ClearSlot();
            }

            return amountItems - itemRemove;
        }
        
        public bool CanCreate() => !IsOccupied && _currentItem == null;

        public int CurrentItemCount() => _currentCountItem;
        
        public bool CheckItemInSlot(string nameItem) => _currentItem != null && _currentItem.nameItem == nameItem;

        public ItemData UnEquipData()
        {
            var item = _currentItem;
            _currentItem = null;
            return item;
        }
        
        private void ClearSlot()
        {
            _currentCountItem = 0;
            _currentItem = null;
        }
    }
}