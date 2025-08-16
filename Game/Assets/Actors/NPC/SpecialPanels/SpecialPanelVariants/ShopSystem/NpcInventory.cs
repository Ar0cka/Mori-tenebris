using System;
using System.Collections.Generic;
using Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop.Data;
using Actors.Player.Inventory;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop
{
    public class NpcInventory : AbstractInventoryLogic
    {
        private NpcInventoryInitConfig _npcInventoryInitConfig;

        public NpcInventory(ISpawnProjectObject itemFactory) : base(itemFactory)
        {
            ItemFactory = itemFactory;
        }
        
        public override void Initialize<TConfig>(TConfig loadConfig)
        {
            BaseInit(loadConfig.SlotParent, loadConfig.InventoryScrObj);

            if (!BaseFunctionInit)
            {
                Debug.LogError("NpcInventory Init Failed");
                return;
            }
            
            if (loadConfig is NpcInventoryInitConfig npcInventoryConfig)
            {
                _npcInventoryInitConfig = npcInventoryConfig;
                AddAllItems(npcInventoryConfig.ShopConfig.ShopConfigData);
            }
        }

        private void AddAllItems(List<ItemInShopConfig> shopConfigs)
        {
            foreach (var shopConfig in shopConfigs)
            {
                var itemInstance = new ItemInstance(shopConfig.itemScrObj.GetItemData());
                AddItemToInventory(itemInstance, shopConfig.countItem);
            }
        }
    }

    public class NpcInventoryInitConfig : BaseInventoryInitializeConfiguration
    {
        public ShopConfig ShopConfig;
        
        public NpcInventoryInitConfig(Transform slotParent, InventoryScrObj inventoryScrObj, ShopConfig shopConfig) : base(slotParent,
            inventoryScrObj)
        {
            SlotParent = slotParent;
            InventoryScrObj = inventoryScrObj;
            ShopConfig = shopConfig;
        }

        public void UpdateShopConfig(ShopConfig shopConfig)
        {
            ShopConfig = shopConfig;
        }
    }
}