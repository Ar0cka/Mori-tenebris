using Enemy;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Player.Inventory
{
    public abstract class ItemAction : MonoBehaviour
    {
        public abstract void Action(ItemInstance itemInstance, ItemActionContext context);
    }
}