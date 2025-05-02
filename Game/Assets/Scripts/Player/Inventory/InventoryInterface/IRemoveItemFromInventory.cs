using Data;

namespace Player.Inventory.InventoryInterface
{
    public interface IRemoveItemFromInventory
    {
        void RemoveItemFromInventory(ItemData itemData, int count);
    }
}