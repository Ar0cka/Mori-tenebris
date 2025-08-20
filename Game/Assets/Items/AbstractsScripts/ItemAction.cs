using Enemy;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Player.Inventory
{
    public abstract class ItemAction : MonoBehaviour
    {
        public abstract int Action(ItemInstance itemInstance);
    }
}