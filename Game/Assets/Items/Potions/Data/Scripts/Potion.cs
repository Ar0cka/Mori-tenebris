using System;

namespace Enemy.ItemTypeData
{
    [Serializable]
    public class Potion : ItemData
    {
        public PotionEffectType PotionEffectType;
        public int amount;
    }
}