using System;
using System.Collections.Generic;
using Actors.Player.Inventory.Enums;
using Actors.Player.Inventory.Scripts.EquipSlots;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using PlayerNameSpace.InventorySystem;
using UnityEngine;
using Zenject;

namespace Actors.Player.Inventory.EquipSlots
{
    public class PlayerEquipSystem : IEquipSlots
    {
        [Inject] private InventoryLogic _inventoryLogic;
        
        private Dictionary<EquipItemType, EquipSlotData> _equipSlot = new();

        public void InitializeEquipSlots(List<GameObject> equipSlots)
        {
            if (equipSlots == null) return;

            #region CreateEquipSlots

            var equipSlotType = Enum.GetValues(typeof(EquipItemType));

            for (int i = 0; i < equipSlots.Count; i++)
            {
                if (equipSlotType.Length <= i) break;

                var itemType = equipSlots[i].gameObject.GetComponent<SlotType>().EquipItemType;
                
                _equipSlot.Add(itemType, new EquipSlotData(itemType, equipSlots[i]));
                Debug.Log("Equip slot name: " + equipSlots[i].name + "Equip slot type = " + itemType);
            }

            #endregion
        }

        public void SelectEquipAction(EquipItemType equipItemType, ItemInstance itemInstance, SlotData slot)
        {
            if (_equipSlot.TryGetValue(equipItemType, out EquipSlotData equipSlotData))
            {
                Debug.Log("Slot type = " + equipItemType);
                EquipItem(itemInstance, equipSlotData, slot);
            }
        }
        
        private void EquipItem(ItemInstance itemInstance, EquipSlotData equipSlotData, SlotData slotWithEquipItem)
        {
            Debug.Log($"Equip Item. item instance type = {itemInstance.itemData.itemTypes}");
            
            if (slotWithEquipItem.CheckItemInSlot(itemInstance.itemID))
            {
                var currentItemData = slotWithEquipItem.TakeItemDataFromSlot();
                var currentItemObject = slotWithEquipItem.TakePrefabFromSlot();

                var item = equipSlotData.EquipItem(currentItemData);
                var prefab = equipSlotData.EquipItemPrefab(currentItemObject);

                if (item == null && prefab == null) return;
                ChangeItemInSlot(slotWithEquipItem, item, prefab);
            }
        }

        private void ChangeItemInSlot(SlotData slotData, ItemInstance itemInstance, GameObject itemPrefab)
        {
            slotData.RegistrateData(itemInstance);
            slotData.ChangeItemSettings(itemPrefab);
            slotData.AddItem(itemInstance, 1);
        }

        public void UnEquipItem(EquipItemType equipItemType, ItemInstance itemData, SlotData freeSlot)
        {
            if (_equipSlot.TryGetValue(equipItemType, out EquipSlotData equipSlotData))
            {
                var item = equipSlotData.UnEquipItem(itemData);
                var prefab = equipSlotData.UnEquipItemObject();

                if (item != null && prefab != null)
                {
                    if (freeSlot.IsEmpty())
                    {
                        ChangeItemInSlot(freeSlot, item, prefab);
                    }
                }
            }
        }
    }
}