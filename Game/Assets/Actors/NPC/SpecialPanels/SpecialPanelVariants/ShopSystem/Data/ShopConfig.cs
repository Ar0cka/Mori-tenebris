using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.NPC.NpcStateSystem.SpecialPanelVariants.Shop.Data
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "NPC/Shop", order = 0)]
    public class ShopConfig : ScriptableObject
    {
        [field:SerializeField] public List<ShopConfigData> ShopConfigData { get; private set; }
    }

    [Serializable]
    public class ShopConfigData
    {
        public ItemScrObj itemScrObj;
        public int countItem;
        
        public ShopConfigData Clone() => (ShopConfigData)MemberwiseClone();
    }
}