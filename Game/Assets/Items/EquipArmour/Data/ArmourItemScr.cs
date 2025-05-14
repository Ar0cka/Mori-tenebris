using System;
using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class ArmourItemScr : MonoBehaviour
    {
        [SerializeField] private ArmourData armourData;
        public ArmourData ArmourData => armourData;
    }

    [Serializable]
    public class ArmourData : ItemData
    {
        public int physicArmour;
        public int magicArmour;
    }
}