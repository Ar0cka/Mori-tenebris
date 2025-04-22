using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace DefaultNamespace.ScriptableObject.Items
{
    public class ArmourItemScr : MonoBehaviour
    {
        [SerializeField] private EquipItem equipItem;
        public EquipItem EquipItem => equipItem;
    }

    [Serializable]
    public class EquipItem : ItemData
    {
        public int physicArmour;
        public int magicArmour;
    }
}