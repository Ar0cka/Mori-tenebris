using System;
using System.Collections.Generic;
using Actors.Player.Inventory;
using Enemy;
using Player.Inventory;
using UnityEngine;

namespace Service
{
    public class InventoryFillService
    {
        public void AddItemToInventory(AbstractInventoryLogic inventoryLogic, List<SaveInventoryData> itemList)
        {
            foreach (var item in itemList)
            {
                inventoryLogic.AddItemToInventory(item.itemInstance, item.amount);
            }
        }

        public void AddItemFromScrObj(AbstractInventoryLogic inventoryLogic, List<ItemConfig> itemList)
        {
            foreach (var item in itemList)
            {
                int itemAmount = item.amount;
                
                while (itemAmount > 0 && inventoryLogic.HaveFreeSlot())
                {
                    ItemData itemData = item.itemScrObj.GetItemData();
                    ItemInstance itemInstance = new ItemInstance(itemData);

                    int addCount = Mathf.Min(itemAmount, itemData.maxStackInSlot);
                    
                    inventoryLogic.AddItemToInventory(itemInstance, addCount);
                    itemAmount -= addCount;
                }
            }
        }
    }
}