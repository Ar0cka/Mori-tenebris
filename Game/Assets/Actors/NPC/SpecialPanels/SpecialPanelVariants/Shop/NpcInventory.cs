using System;
using System.Collections.Generic;
using Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop.Data;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop
{
    public class NpcInventory : IDisposable
    {
        private Transform _inventoryParent;
        private ShopConfig _currentShopConfig;

        private IInventoryAdder _playerInventory;

        public Action<ItemInstance, int> OnItemBuys;
        
        public NpcInventory(ShopConfig shopConfig)
        {
            _currentShopConfig = shopConfig;
        }

        public void Initialize()
        {
            OnItemBuys += BuyItem;
        }

        public void OpenShop(IInventoryAdder inventoryAdder)
        {
            _playerInventory = inventoryAdder;
        }

        private void BuyItem(ItemInstance itemInstance, int amount)
        {
            _playerInventory.AddItemToInventory(itemInstance, amount);
        }

        private void InitItemsInstance()
        {
            for (int i = 0; i < _currentShopConfig.ShopConfigData.Count; i++)
            {
                
            }
        }

        public void Dispose()
        {
            OnItemBuys -= BuyItem;
        }
    }
}