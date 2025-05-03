using System;
using UnityEngine;

namespace PlayerNameSpace.Inventory
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private int countSlots;
        
        public GameObject SlotPrefab => slotPrefab;
        public int CountSlots => countSlots;

        public void UpgradeSlotCapacity(int amount)
        {
            countSlots += amount;
        }
        
        public InventoryData Clone()
        {
            return new InventoryData()
            {
                slotPrefab = slotPrefab,
                countSlots = countSlots
            };
        }
    }
}