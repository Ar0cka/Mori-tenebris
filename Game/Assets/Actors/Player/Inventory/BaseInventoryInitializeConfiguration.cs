using System.Collections.Generic;
using UnityEngine;

namespace Actors.Player.Inventory
{
    public abstract class BaseInventoryInitializeConfiguration
    {
        public BaseInventoryInitializeConfiguration(Transform parent, InventoryScrObj inventoryScrObj)
        {
            SlotParent = parent;
            InventoryScrObj = inventoryScrObj;
        }

        public Transform SlotParent;
        public InventoryScrObj InventoryScrObj;
    }

    public class InventoryInitializeConfig : BaseInventoryInitializeConfiguration
    {
        public InventoryInitializeConfig(Transform parent, InventoryScrObj inventoryScrObj,
            List<GameObject> slotContent) : base(parent, inventoryScrObj)
        {
            SlotParent = parent;
            InventoryScrObj = inventoryScrObj;
            EquipSlotContent = slotContent;
        }
        
        public List<GameObject> EquipSlotContent;
    }
}