using System.Collections;
using System.Collections.Generic;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using Zenject;

public class TakeItemInInventory : MonoBehaviour
{
    [Inject] private IInventoryAdder _inventoryAdder;

    public void TakeItem(ItemInstance itemInstance, int count)
    {
        _inventoryAdder.AddItemToInventory(itemInstance, count);
    }
}
