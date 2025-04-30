using System.Collections.Generic;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Inventory
{
    public class Inventory : MonoBehaviour, IAddNewItemOnInventory
    {
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
                _slots.Add(new SlotData(prefab));
            }
        }

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
                        remainingCount = slot.CreateObjectInSlot(itemData, remainingCount);
                    }
                }
            }
            
            return remainingCount;
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