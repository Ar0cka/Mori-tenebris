using System;

namespace Enemy.ItemTypeData
{
    [Serializable]
    public class Potion
    {
        public PotionEffectType PotionEffectType;
        public int amount;
    }
}