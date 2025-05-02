namespace Player.Inventory.InventoryInterface
{
    public interface IFindItemInInventory
    {
        SlotData FindItemInInventory(string itemName);
    }
}