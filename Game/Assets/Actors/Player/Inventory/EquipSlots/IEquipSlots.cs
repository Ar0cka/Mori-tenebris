using Actors.Player.Inventory.Enums;
using Player.Inventory;
using PlayerNameSpace.InventorySystem;

namespace Actors.Player.Inventory.EquipSlots
{
    public interface IEquipSlots
    { 
        void SelectEquipAction(EquipItemType equipItemType, ItemInstance itemInstance, SlotData freeSlot);
        void UnEquipItem(EquipItemType equipItemType, ItemInstance itemData, SlotData freeSlot);
    }
}