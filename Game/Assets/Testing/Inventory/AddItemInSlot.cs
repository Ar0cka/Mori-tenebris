using DefaultNamespace.Enums;
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
    [Inject] private ITakeDamage _takeDamage;

    public void ClickAndAdd()
    {
        _takeDamage.TakeDamage(25, DamageType.Physic);
        _addItemOnInventoryAdder.AddItemToInventory(itemScrObj.ItemData, amount);
    }

    public void RemoveItemFromSlot()
    {
        _inventoryRemove.RemoveItem(itemScrObj.ItemData, amount);
    }
}
