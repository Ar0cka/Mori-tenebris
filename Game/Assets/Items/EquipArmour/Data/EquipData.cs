using System;
using Actors.Player.Inventory.Enums;
using Enemy;

namespace Items.EquipArmour.Data
{
    [Serializable]
    public class EquipData : ItemData
    {
        public EquipItemType equipItemType;
    }
}