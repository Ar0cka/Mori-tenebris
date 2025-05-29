using System;
using UnityEngine.Serialization;

namespace Enemy.ItemTypeData
{
    [Serializable]
    public class Potion : ItemData
    {
        public PotionEffectType PotionEffectType;
        public int healthAmount;
    }
}