using DefaultNamespace.Enums;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

public class AddItemInSlot : MonoBehaviour
{
    [SerializeField] private ItemScrObj itemScrObj;
    [SerializeField] private int amount;
    
    [Inject] private IInventoryAdder _addItemOnInventoryAdder;
    [Inject] private IInventoryRemove _inventoryRemove;
    [Inject] private IHitPlayer _hitPlayer;

    public void ClickAndAdd()
    {
        ItemInstance itemInstance = new ItemInstance(itemScrObj.GetItemData());
        _addItemOnInventoryAdder.AddItemToInventory(itemInstance, amount);
    }
}
