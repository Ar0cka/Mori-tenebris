using UnityEngine;
using Data;

namespace Player.Inventory
{
    public class SlotData
    {
        private int _currentCountItemInSlot;

        public bool IsOccupied { get; private set; }
        public bool IsFull { get; private set; }
        public GameObject SlotPrefab { get; private set; }

        private ItemData _currentItemInSlot;
        private ItemSettings _itemSettings;

        public SlotData(GameObject slotObject)
        {
            IsOccupied = false;
            IsFull = false;
            SlotPrefab = slotObject;
        }

        #region CreateItem

        public int CreateObjectInSlot(ItemData itemData, int countItemAdd)
        {
            if (IsFreeSlot())
            {
                _currentItemInSlot = itemData;
                IsOccupied = true;

                _itemSettings = SlotPrefab.GetComponentInChildren<ItemSettings>();

                int result = AddItemInSlot(itemData, countItemAdd);

                return result;
            }

            return countItemAdd;
        }

        #endregion

        #region AddItem

        public int AddItemInSlot(ItemData itemData, int countAdd)
        {
            if (itemData == null) return countAdd;

            if (_currentItemInSlot.nameItem == itemData.nameItem && !IsFull)
            {
                int space = itemData.maxStackInSlot - _currentCountItemInSlot;
                int addItemCount = Mathf.Min(countAdd, space);

                _currentCountItemInSlot += addItemCount;

                IsFull = _currentCountItemInSlot == itemData.maxStackInSlot;

                _itemSettings.UpdateUI(_currentCountItemInSlot);

                return countAdd - addItemCount;
            }

            return countAdd;
        }

        #endregion

        #region RemoveItem

        public int RemoveItemInSlot(ItemData itemData, int countRemove)
        {
            if (itemData == null) return countRemove;

            int itemRemove = Mathf.Min(countRemove, _currentCountItemInSlot);

            _currentCountItemInSlot -= itemRemove;

            _itemSettings.UpdateUI(_currentCountItemInSlot);

            CheckStatesSlot(itemData);

            if (!IsOccupied)
            {
                _itemSettings.DeleteObjectFromSlot();
                _itemSettings = null;
                _currentItemInSlot = null;
            }

            return itemRemove - _currentCountItemInSlot;
        }

        private void CheckStatesSlot(ItemData itemData)
        {
            IsOccupied = _currentCountItemInSlot == 0;
            IsFull = _currentCountItemInSlot != itemData.maxStackInSlot;
        }

        #endregion

        #region Getters

        public ItemData ItemDataInSlot()
        {
            return _currentItemInSlot;
        }

        public bool IsFreeSlot()
        {
            return !IsOccupied && !IsFull;
        }

        #endregion
    }
}