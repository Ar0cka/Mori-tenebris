using UnityEngine;

namespace Project.Service.Context
{
    public class CraftRendInitContext
    {
        public readonly InventoryScrObj InventoryConfig;
        public readonly Transform LeftInventory;

        public CraftRendInitContext(InventoryScrObj inventoryConfig, Transform leftInventory)
        {
            InventoryConfig = inventoryConfig;
            LeftInventory = leftInventory;
        }
    }
}