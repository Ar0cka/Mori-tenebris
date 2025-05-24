using Enemy;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace Items.EquipArmour
{
    public class EquipAction : ItemAction
    {
        [Inject] private InventoryLogic _inventoryLogic;

        public override void EquipItem(ItemScrObj itemScr, bool isEquip)
        {
            if (isEquip)
            {
                itemScr.OnUnequip(_inventoryLogic);
            }
            else
            {
                itemScr.OnEquip(_inventoryLogic);
            }
        }
    }
}