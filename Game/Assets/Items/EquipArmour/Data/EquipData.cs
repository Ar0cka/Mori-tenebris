using System;
using Actors.Player.Inventory.Enums;
using Actors.Player.Inventory.EquipSlots;
using Enemy;
using UnityEngine;

namespace Items.EquipArmour.Data
{
    [Serializable]
    public class EquipData : ItemData, IEquipable
    {
        public EquipItemType equipItemType { get; private set;}

        [SerializeField] private bool _isEquipped = false;
        public bool GetCurrentEquipStatus() => _isEquipped;
        public void ChangeEquipStatus(bool status)
        {
            _isEquipped = status;
        }
    }

    public interface IEquipable
    {
        public EquipItemType equipItemType { get; }
        bool GetCurrentEquipStatus();
        void ChangeEquipStatus(bool status);
    }
}