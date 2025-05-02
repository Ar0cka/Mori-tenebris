using Data;

namespace Player.Inventory.InventoryInterface
{
    public interface IInventoryAdder
    {
        void AddItemToInventory(ItemData itemData, int amount);
    }
}