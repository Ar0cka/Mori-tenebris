using System;

namespace Items.Data.Scripts
{
    [Serializable]
    public class ItemTradeInfo
    {
        public ItemRarity rarity;
        public int price;
    }

    public enum ItemRarity
    {
        Сommon,
        Rare,
        Epic,
        Legendary,
        Dark,
        Mythic,
        Quest
    }
}