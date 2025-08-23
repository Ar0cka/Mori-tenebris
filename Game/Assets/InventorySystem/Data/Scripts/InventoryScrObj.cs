using System.Collections.Generic;
using PlayerNameSpace.Inventory;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "Create/Inventory", order = 0)]
public class InventoryScrObj : ScriptableObject
{
    [SerializeField] private InventoryData inventoryData;
    public InventoryData InventoryData => inventoryData;
}