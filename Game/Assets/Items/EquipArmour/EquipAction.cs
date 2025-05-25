using Enemy;
using Items.EquipArmour.Data;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace Items.EquipArmour
{
    public class EquipAction : ItemAction
    {
        [Inject] private InventoryLogic _inventoryLogic;

        public override void EquipItem(ItemInstance itemInstance, bool isEquip)
        {
            if (itemInstance.itemData is EquipData equipData)
            {
                if (isEquip)
                {
                    _inventoryLogic.UnEquipItem(equipData.equipItemType, itemInstance);
                }
                else
                {
                    _inventoryLogic.SelectEquipAction(equipData.equipItemType, itemInstance);
                }
            }
        }
    }
}