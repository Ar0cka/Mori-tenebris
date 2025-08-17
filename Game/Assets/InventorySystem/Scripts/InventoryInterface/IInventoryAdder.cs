using Enemy;

namespace Player.Inventory.InventoryInterface
{
    public interface IInventoryAdder
    {
        void AddItemToInventory(ItemInstance itemInstance, int amount);
    }
}