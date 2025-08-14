using Actors.Player.Inventory.EquipSlots;
using Enemy;
using Items.EquipArmour.Data;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using PlayerNameSpace.InventorySystem;
using UnityEngine;
using Zenject;

namespace Items.EquipArmour
{
    public class EquipAction : ItemAction
    {
        [Inject] private IEquipSlots _equipSystem;

        public override void EquipItem(ItemInstance itemInstance, bool isEquip, SlotData slotData)
        {
            if (itemInstance.itemData is EquipData equipData)
            {
                if (isEquip)
                {
                    _equipSystem.UnEquipItem(equipData.equipItemType, itemInstance, slotData);
                }
                else
                {
                    _equipSystem.SelectEquipAction(equipData.equipItemType, itemInstance, slotData);
                }
            }
        }
    }
}