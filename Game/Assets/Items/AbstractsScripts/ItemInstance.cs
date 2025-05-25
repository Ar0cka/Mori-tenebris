using System;
using DefaultNamespace.Enums;
using Enemy;
using Random = UnityEngine.Random;

namespace Player.Inventory
{
    [Serializable]
    public class ItemInstance
    {
        public string itemID;
        public ItemData itemData;
        public int maxStack = 0;
        public int amount = 0;

        public ItemInstance(ItemData itemData)
        {
            itemID = GenerateID(itemData.itemTypes);
            this.itemData = itemData;
            maxStack = itemData.maxStackInSlot;
        }

        private string GenerateID(ItemTypes itemType)
        {
            string id = "";

            int randomCount = Random.Range(0, 10000);
            
            switch (itemType)
            {
                case ItemTypes.Collectable:
                    id = $"C:{randomCount}";
                    break;
                case ItemTypes.Equip:
                    id = $"E:{randomCount}";
                    break;
                case ItemTypes.Other:
                    id = $"O:{randomCount}";
                    break;
                default:
                    id = $"I:{randomCount}";
                    break;
            }
            
            return id;
        }
    }
}