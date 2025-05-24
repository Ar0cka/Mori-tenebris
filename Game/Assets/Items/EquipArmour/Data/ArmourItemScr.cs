using System;
using DefaultNamespace.Enums;
using Items.EquipArmour.Data;
using PlayerNameSpace.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Armour", menuName = "EquipItem/Armour")]
    public class ArmourItemScr : ItemScrObj
    {
        [SerializeField] private ArmourData armourData;
        public ArmourData ArmourData => armourData;
        
        public override void OnEquip(InventoryLogic inventoryLogic)
        {
            inventoryLogic.SelectEquipAction(armourData.equipItemType, armourData);
        }

        public override void OnUnequip(InventoryLogic inventoryLogic)
        {
            inventoryLogic.UnEquipItem(armourData.equipItemType, armourData);
        }
        
        public override ItemData GetItemData()
        {
            return armourData;
        }
    }

    [Serializable]
    public class ArmourData : EquipData
    {
        public int physicArmour;
        public int magicArmour;
    }
}