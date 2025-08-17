using Actors.NPC.NpcStateSystem;
using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using Items.EquipArmour.Data;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using Zenject;

namespace Items
{
    public class ItemRouterService
    {
        private const int EquipItemCount = 1;
        
        public void TransitItem(AbstractInventoryLogic fromInventory, AbstractInventoryLogic toInventory, ItemInstance itemInstance, int amount)
        {
            fromInventory.RemoveItem(itemInstance, amount);
            toInventory.AddItemToInventory(itemInstance, amount);
        }

        public void EquipItem(AbstractInventoryLogic fromInventory, IEquipSlots equipSystem, ItemInstance itemInstance)
        {
            if (itemInstance.itemData is EquipData equipData)
            {
                equipSystem.SelectEquipAction(equipData.equipItemType, itemInstance, fromInventory.FindItem(itemInstance));
            }
        }

        public void RemoveItem(IInventoryRemove fromInventory, ItemInstance itemInstance, int amount)
        {
            fromInventory.RemoveItem(itemInstance, amount);
        }

        public void UnEquipItem(AbstractInventoryLogic toInventory, IEquipSlots equipSystem, ItemInstance itemInstance)
        {
            if (itemInstance.itemData is EquipData equipData)
            {
                equipSystem.UnEquipItem(equipData.equipItemType, itemInstance, toInventory.GetFirstFreeSlot());
            }
        }
    }
}