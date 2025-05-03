using PlayerNameSpace.Inventory;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "Create/Inventory", order = 0)]
public class InventoryScrObj : ScriptableObject
{
    [SerializeField] private InventoryData inventoryData;
    public InventoryData InventoryData => inventoryData;
}