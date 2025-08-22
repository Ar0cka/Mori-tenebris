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
        
        public override ItemData GetItemData()
        {
            return (ItemData)armourData.Clone();
        }
    }

    [Serializable]
    public class ArmourData : EquipData
    {
        public int physicArmour;
        public int magicArmour;
    }
}