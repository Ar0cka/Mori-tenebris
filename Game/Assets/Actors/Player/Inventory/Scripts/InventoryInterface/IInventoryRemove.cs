using Enemy;

namespace Player.Inventory.InventoryInterface
{
    public interface IInventoryRemove
    {
        void RemoveItem(ItemInstance itemInstance, int count);
    }
}