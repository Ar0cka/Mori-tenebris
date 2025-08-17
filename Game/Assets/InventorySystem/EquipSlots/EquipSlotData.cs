using System;
using Actors.Player.Inventory.Enums;
using Enemy;
using EventBusNamespace;
using Items.EquipArmour.Data;
using Player.Inventory;
using SlotSystem;
using UnityEngine;

namespace Actors.Player.Inventory.Scripts.EquipSlots
{
    public class EquipSlotData
    {
        private EquipSlot _equipSlot;
        private ItemSettings _itemSettings;
        private EquipItemType _equipItemType;
        private GameObject _equipSlotGameObject;
        private SlotView _slotView;

        private GameObject _itemPrefab;


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
                _itemPrefab = itemObject;
                _itemSettings = itemObject.GetComponent<ItemSettings>();
                itemObject.transform.SetParent(_equipSlotGameObject.transform);
                itemObject.transform.position = _equipSlotGameObject.transform.position;
            }
        }

        public ItemInstance EquipItem(ItemInstance itemData)
        {
            try
            {
                var item = _equipSlot.EquipItem(itemData);
                return item;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public GameObject EquipItemPrefab(GameObject itemPrefab)
        {
            try
            {
                if (_itemPrefab != null)
                {
                    var prefab = UnEquipItemObject();
                    _itemPrefab = prefab;
                    SetupItemSettings(itemPrefab);
                    return prefab;
                }
                else
                {
                    _itemPrefab = itemPrefab;
                    SetupItemSettings(itemPrefab);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public ItemInstance UnEquipItem(ItemInstance itemData)
        {
            var data = _equipSlot.UnEquipItem(itemData);

            if (data != null)
            {
                return data;
            }

            return null;
        }

        public GameObject UnEquipItemObject()
        {
            if (_itemPrefab != null)
            {
                var item = _itemPrefab;
                _itemPrefab = null;
                return item;
            }
            
            Debug.LogError("Dont find item prefab");
            return null;
        }
        
        public bool IsEquipped() => _equipSlot.IsEquipped;
    }
}