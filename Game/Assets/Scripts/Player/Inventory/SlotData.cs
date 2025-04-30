using System;
using EventBusNamespace;
using UnityEngine;

namespace Player.Inventory
{
    public class SlotData
    {
        private int _currentCountItemInSlot;
        
        public bool IsOccupied { get; private set; }
        public  bool IsFull { get; private set; }

        private GameObject _slotObject;
        private ItemData _currentItemInSlot;
        private SlotSettings _slotSettings;

        public SlotData(GameObject slotObject)
        {
            IsOccupied = false;
            IsFull = false;
            _slotObject = slotObject;
            
            _slotSettings = slotObject.GetComponent<SlotSettings>();
        }

        public int CreateObjectInSlot(ItemData itemData, int countItemAdd)
        {
            if (IsFreeSlot())
            {
                _currentItemInSlot = itemData;
                
                IsOccupied = true;
                
                int result = AddItemInSlot(itemData, countItemAdd);
                
                _slotSettings.AddNewItem(itemData, this);
                
                return result;
            }

            return countItemAdd;
        }
        
        public int AddItemInSlot(ItemData itemData, int countAdd)
        {
            if (itemData == null) return countAdd;

            if (_currentItemInSlot.nameItem == itemData.nameItem && !IsFull)
            {
                int space = itemData.maxStackInSlot - _currentCountItemInSlot;
                int addItemCount = Mathf.Min(countAdd, space);
                
                _currentCountItemInSlot += addItemCount;

                IsFull = _currentCountItemInSlot == itemData.maxStackInSlot;
                
                EventBus.Publish(new SendUpdateSlotUI(_currentCountItemInSlot));
                
                return countAdd - addItemCount;
            }

            return countAdd;
        }

        public ItemData ItemDataInSlot()
        {
            return _currentItemInSlot;
        }
        
        public bool IsFreeSlot()
        {
            return !IsOccupied && !IsFull;
        }
    }
}