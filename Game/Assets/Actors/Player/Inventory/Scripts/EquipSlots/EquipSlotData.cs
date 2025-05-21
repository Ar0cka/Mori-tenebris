using System;
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
        private EquipItemType _equipItemType;
        private GameObject _equipSlotGameObject;

        public EquipSlotData(EquipItemType equipItemType, GameObject slotObject)
        {
            _equipItemType = equipItemType;
            _equipSlot = new EquipSlot(_equipItemType);
            _equipSlotGameObject = slotObject;
        }
        
        private void SetupItemSettings(GameObject itemObject)
        {
            if (itemObject != null)
            {
                _itemSettings = itemObject.GetComponent<ItemSettings>();
                itemObject.transform.SetParent(_equipSlotGameObject.transform);
                itemObject.transform.position = _equipSlotGameObject.transform.position;
            }
        }

        public bool EquipItem(GameObject itemObject, ItemData itemData)
        {
            try
            {
                _equipSlot.EquipItem(itemData);
                SetupItemSettings(itemObject);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }
        
        public bool IsEquipped() => _equipSlot.IsEquipped;
    }
}