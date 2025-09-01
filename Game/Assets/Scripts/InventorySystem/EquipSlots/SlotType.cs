using Actors.Player.Inventory.Enums;
using UnityEngine;

namespace Actors.Player.Inventory.EquipSlots
{
    public class SlotType : MonoBehaviour
    {
        [field: SerializeField] public EquipItemType EquipItemType { get; private set; }
    }
}