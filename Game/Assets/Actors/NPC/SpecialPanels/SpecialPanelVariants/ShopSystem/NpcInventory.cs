using System;
using System.Collections.Generic;
using Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop.Data;
using Actors.Player.Inventory;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop
{
    public class NpcInventory : AbstractInventoryLogic
    {
        public override void Initialize(Transform slotParent, InventoryScrObj inventoryScrObj)
        {
            BaseInit(slotParent, inventoryScrObj);
        }
    }
}