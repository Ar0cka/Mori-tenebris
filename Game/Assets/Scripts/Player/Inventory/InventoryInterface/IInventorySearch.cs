using PlayerNameSpace.Inventory;

namespace Player.Inventory.InventoryInterface
{
    public interface IInventorySearch
    {
        public SlotData FindItemOnInventory(string nameItem);
    }
}