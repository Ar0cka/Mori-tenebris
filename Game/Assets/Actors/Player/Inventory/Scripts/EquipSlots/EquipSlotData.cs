using Actors.Player.Inventory.Enums;
using Enemy;
using EventBusNamespace;
using Items.EquipArmour.Data;
using Player.Inventory;
using UnityEngine;

namespace Actors.Player.Inventory.Scripts.EquipSlots
{
    public class EquipSlotData
    {
        private EquipSlot _equipSlot;
        private ItemSettings _itemSettings;
        private EquipSlotType _equipSlotType;

        public EquipSlotData(EquipSlotType equipSlotType)
        {
            _equipSlotType = equipSlotType;
            _equipSlot = new EquipSlot(_equipSlotType);
        }
        
        private void SetupItemSettings(GameObject itemObject)
        {
            if (itemObject != null)
            {
                _itemSettings = itemObject.GetComponent<ItemSettings>();
            }
        }

        public void EquipItem(GameObject itemObject, ItemData itemData)
        {
            _equipSlot.EquipItem(itemData);
            SetupItemSettings(itemObject);
        }
        
        public bool IsEquipped() => _equipSlot.IsEquipped;
    }
}