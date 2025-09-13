using UnityEngine;

namespace Project.Service.Context
{
    public class InventoryRendererInitContext
    {
        public readonly InventoryScrObj ShopInventoryConfig;
        public readonly Transform LeftInventory;
        public readonly Transform RightInventory;

        public InventoryRendererInitContext(InventoryScrObj shopInventoryConfig, Transform leftInventory,
            Transform rightInventory)
        {
            ShopInventoryConfig = shopInventoryConfig;
            LeftInventory = leftInventory;
            RightInventory = rightInventory;
        }
    }
}