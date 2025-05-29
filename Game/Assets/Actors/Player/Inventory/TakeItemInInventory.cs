using System;
using System.Collections;
using System.Collections.Generic;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using UnityEditor;
using UnityEngine;
using Zenject;

public class TakeItemInInventory : MonoBehaviour
{
    [SerializeField] private int countTakeItems = 1;
    [SerializeField] private float overlapRadius = 1.5f;
    [Inject] private IInventoryAdder _inventoryAdder;

    public bool TakeItem(PutItem item)
    {
        if (item == null) return false;

        Debug.Log($"item instance = {item.ItemInstance}, itemCount = {item.Count}");
        _inventoryAdder.AddItemToInventory(item.ItemInstance, item.Count);

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var items = Physics2D.OverlapCircleAll(transform.position, overlapRadius, LayerMask.GetMask("Item"));
            
            if (items.Length == 0) return;

            foreach (var item in items)
            {
                if (item.CompareTag("Item"))
                {
                    var takeItemsComponent = item.gameObject.GetComponent<TakeItems>();

                    if (takeItemsComponent != null)
                    {
                        bool isAdd = TakeItem(takeItemsComponent.TakeItem());

                        if (isAdd)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }
}