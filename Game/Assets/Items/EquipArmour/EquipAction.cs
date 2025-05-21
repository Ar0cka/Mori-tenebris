using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace Items.EquipArmour
{
    public class EquipAction : ItemAction
    {
        [Inject] private InventoryLogic _inventoryLogic;

        public override void ActionItem(ItemScrObj itemScr)
        {
            itemScr.OnEquip(_inventoryLogic);
        }
    }
}