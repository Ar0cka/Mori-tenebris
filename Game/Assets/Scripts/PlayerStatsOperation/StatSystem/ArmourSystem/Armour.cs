using System.Collections.Generic;
using DefaultNamespace.Enums;
using DefaultNamespace.ScriptableObject.Items;

namespace DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem
{
    public class Armour : IEquipAndUnEquipItem
    {
        public int PhysicArmour { get; private set; }
        public int MagicArmour { get; private set; }

        private Dictionary<string, EquipItem> _equipItems = new Dictionary<string, EquipItem>();

        public void EquipArmourItem(ItemData itemData)
        {
            if (itemData is EquipItem equipItem)
            {
                _equipItems.Add(itemData.nameItem, equipItem);

                PhysicArmour += equipItem.physicArmour;
                MagicArmour += equipItem.magicArmour;
            }
            
        }

        public void DeleteArmour(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (_equipItems.TryGetValue(name, out EquipItem item))
                {
                    PhysicArmour -= item.physicArmour;
                    MagicArmour -= item.magicArmour;
                }
                _equipItems.Remove(name);
            }
        }
    }
}