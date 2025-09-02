using System.Collections.Generic;
using UnityEngine;

namespace Actors.Player.Inventory
{
    public abstract class BaseInventoryInitializeConfiguration
    {
        public BaseInventoryInitializeConfiguration(Transform parent, InventoryScrObj inventoryScrObj, InventoryObjectType type)
        {
            SlotParent = parent;
            InventoryScrObj = inventoryScrObj;
            InventoryObjectType = type;
        }

        public Transform SlotParent;
        public InventoryScrObj InventoryScrObj;
        public InventoryObjectType InventoryObjectType;
    }

    public class InventoryInitializeConfig : BaseInventoryInitializeConfiguration
    {
        public InventoryInitializeConfig(Transform parent, InventoryScrObj inventoryScrObj, InventoryObjectType type) : base(parent, inventoryScrObj, type)
        {
           
        }
    }
}