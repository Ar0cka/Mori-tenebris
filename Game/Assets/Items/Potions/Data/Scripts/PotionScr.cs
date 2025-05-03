using Data.ItemTypeData;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "potion", menuName = "Create/newPotion", order = 0)]
    public class PotionScr : ItemScrObj
    {
        [SerializeField] private Potion potion;
        public Potion Potion => potion;
    }
}