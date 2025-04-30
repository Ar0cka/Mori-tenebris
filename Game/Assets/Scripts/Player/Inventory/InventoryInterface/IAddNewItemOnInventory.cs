namespace Player.Inventory.InventoryInterface
{
    public interface IAddNewItemOnInventory
    {
        void AddItemInInventory(ItemData itemData, int count);
    }
}