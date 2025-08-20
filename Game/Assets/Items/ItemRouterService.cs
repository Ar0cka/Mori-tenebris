using Actors.NPC.NpcStateSystem;
using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using Items.EquipArmour.Data;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using UnityEngine;

namespace Items
{
    public class ItemRouterService
    {
        private const int DefaultItemRemove = 1;
        
        public void TransitItem(AbstractInventoryLogic fromInventory, AbstractInventoryLogic toInventory, ItemInstance itemInstance, int amount)
        {
            fromInventory.RemoveItem(itemInstance, amount);
            toInventory.AddItemToInventory(itemInstance, amount);
        }

        public void EquipItem(AbstractInventoryLogic inventory, IEquipSlots equipSystem, ItemInstance itemInstance)
        {
            if (itemInstance.itemData is IEquipable equipData)
            {
                if (!equipData.GetCurrentEquipStatus())
                {
                    equipSystem.SelectEquipAction(equipData.equipItemType, itemInstance, inventory.GetSlotFromInventory(itemInstance));
                    Debug.Log("Use item equipment");
                }
                else
                {
                    equipSystem.UnEquipItem(equipData.equipItemType, itemInstance, inventory.GetFirstFreeSlot());
                    Debug.Log("Use item unequipment");
                }
            }
        }

        public void RemoveItem(IInventoryRemove fromInventory, ItemInstance itemInstance, int amount = DefaultItemRemove)
        {
            fromInventory.RemoveItem(itemInstance, amount);
        }
    }
}