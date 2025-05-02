using System.Collections.Generic;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using UnityEngine.Serialization;
using Data;
using DefaultNamespace.Zenject;
using Zenject;

namespace Player.Inventory
{
    public class Inventory : MonoBehaviour, IAddNewItemOnInventory, IRemoveItemFromInventory, IFindItemInInventory
    {
        [Inject] private IItemsFactory itemsFactory;
        
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotParent;

        [FormerlySerializedAs("coutSlot")] [SerializeField]
        private int countSlot;

        private List<SlotData> _slots = new List<SlotData>();

        public void Initialize()
        {
            for (int i = 0; i < countSlot; i++)
            {
                var prefab = Instantiate(slotPrefab, slotParent, false);
                prefab.name = "Slot" + i;
                _slots.Add(new SlotData(prefab));
            }
        }

        #region AddItem

        public void AddItemInInventory(ItemData itemData, int count)
        {
            if (itemData == null || string.IsNullOrEmpty(itemData.nameItem)) return;

            if (IsFullInventory())
            {
                Debug.Log("Inventory is full");
                return;
            }

            int remainingCount = count;

            remainingCount = AddItem(itemData, remainingCount);

            if (remainingCount != 0)
            {
                Debug.Log($"Инвентарь заполнен. Не поместилось {remainingCount} с именем {itemData.nameItem}");
            }
        }

        private int AddItem(ItemData itemData, int count)
        {
            int remainingCount = count;

            foreach (var slot in _slots)
            {
                if (remainingCount <= 0) break;

                if (slot.ItemDataInSlot() == itemData && !slot.IsFull)
                {
                    remainingCount = slot.AddItemInSlot(itemData, remainingCount);
                }
            }

            if (remainingCount > 0)
            {
                foreach (var slot in _slots)
                {
                    if (remainingCount <= 0) break;

                    if (!slot.IsOccupied)
                    {
                        #region InjectCreateItem

                        itemsFactory.Create(itemData.prefabItem, slot.SlotPrefab.transform);

                        #endregion
                        
                        remainingCount = slot.CreateObjectInSlot(itemData, remainingCount);
                    }
                }
            }

            return remainingCount;
        }

        #endregion

        #region RemoveItem

        public void RemoveItemFromInventory(ItemData itemData, int count)
        {
            if (itemData == null || string.IsNullOrEmpty(itemData.nameItem)) return;

            int remainingCount = count;

            foreach (var slot in _slots)
            {
                if (slot.ItemDataInSlot().nameItem == itemData.nameItem)
                {
                    remainingCount = slot.RemoveItemInSlot(itemData, remainingCount);
                }

                if (remainingCount == 0)
                {
                    break;
                }
            }

            if (remainingCount != 0)
            {
                Debug.Log("Dont find object");
            }
        }

        #endregion

        public SlotData FindItemInInventory(string itemName)
        {
            foreach (var slot in _slots)
            {
                Debug.Log($"Item in slot name = {slot.ItemDataInSlot().nameItem} and find item name {itemName}");
                
                if (slot.ItemDataInSlot().nameItem == itemName)
                {
                    return slot;
                }
            }
            
            return null;
        }
        
        private bool IsFullInventory()
        {
            foreach (var slot in _slots)
            {
                if (slot.IsFreeSlot()) return false;
            }

            return true;
        }
    }
}