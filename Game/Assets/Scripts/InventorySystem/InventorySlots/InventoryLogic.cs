using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Actors.Player.Inventory;
using Actors.Player.Inventory.Enums;
using Actors.Player.Inventory.Scripts.EquipSlots;
using DefaultNamespace.Enums;
using Enemy;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using SlotSystem;
using Systems.DataLoader.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace PlayerNameSpace.Inventory
{
    public class InventoryLogic : AbstractInventoryLogic
    {
        private InventoryScrObj _inventoryScrObj;
        private GameObject _slotPrefab;
        private Transform _slotParent;
        private int _capacityInventory;
        
        public InventoryLogic(ISpawnProjectObject itemFactory) : base(itemFactory)
        {
            ItemFactory = itemFactory;
        }

        public override void Initialize<TConfig>(TConfig loadConfig)
        {
            if (loadConfig is InventoryInitializeConfig inventoryConfig)
            {
                BaseInit(inventoryConfig.SlotParent, inventoryConfig.InventoryScrObj);
            }
        }
    }
}