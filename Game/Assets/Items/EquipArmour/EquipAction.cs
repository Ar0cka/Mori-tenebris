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
        [Inject] private InventoryLogic _inventoryLogic;
        public bool IsEquipped { get; private set; }

        public override void EquipItem(ItemInstance itemInstance, SlotData slotData)
        {
            if (itemInstance.itemData is EquipData equipData)
            {
                if (IsEquipped)
                {
                    _equipSystem.UnEquipItem(equipData.equipItemType, itemInstance, slotData);
                    IsEquipped = false;
                }
                else
                {
                    _equipSystem.SelectEquipAction(equipData.equipItemType, itemInstance, slotData);
                    IsEquipped = true;
                }
            }
        }
    }
}