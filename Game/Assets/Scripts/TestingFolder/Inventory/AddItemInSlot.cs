using DefaultNamespace.Enums;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

public class AddItemInSlot : MonoBehaviour
{
    [SerializeField] private ItemScrObj itemScrObj;
    [SerializeField] private int amount;
    
    [Inject] private IAddNewItemOnInventory _addItemOnInventory;
    [Inject] private ITakeDamage _takeDamage;

    public void ClickAndAdd()
    {
        _takeDamage.TakeDamage(25, DamageType.Physic);
        _addItemOnInventory.AddItemInInventory(itemScrObj.ItemData, amount);
    }
}
