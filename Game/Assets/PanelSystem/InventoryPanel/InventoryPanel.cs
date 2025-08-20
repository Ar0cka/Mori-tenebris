using System.Collections.Generic;
using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using DefaultNamespace.Enums;
using Enemy;
using Items;
using Items.EquipArmour.Data;
using Items.Potions.Scripts;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.InventoryPanel
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> equipSlots;
        [SerializeField] private TakeItemInInventory takeItemInInventory;
        
        [SerializeField] private GameObject inventoryObject;
        [SerializeField] private ItemPanelSystem itemPanelSystem;
        
        [SerializeField] private Transform slotContent;
        [SerializeField] private InventoryScrObj inventoryConfig;
        
        [Inject] private PanelController _panelController;
        [Inject] private ItemRouterService _itemRouterService;
        [Inject] private DiContainer _diContainer;
        
        private AbstractInventoryLogic _inventoryLogic;
        private PlayerEquipSystem _equipSlots;

        public void Initialize()
        {
            _inventoryLogic = _diContainer.Instantiate<InventoryLogic>();
            _equipSlots = _diContainer.Instantiate<PlayerEquipSystem>();
            
            _inventoryLogic.Initialize(new InventoryInitializeConfig(slotContent, inventoryConfig));
            _equipSlots.InitializeEquipSlots(equipSlots);
            takeItemInInventory.Initialize(_inventoryLogic);
        }

        public void ItemRoute(ItemUI itemUI)
        {
            var itemInstance = itemUI.GetItemInstance();
            
            switch (itemInstance.itemData.itemTypes)
            {
                case ItemTypes.Equip:
                    _itemRouterService.EquipItem(_inventoryLogic, _equipSlots, itemInstance);
                    break;
                case ItemTypes.Collectable: 
                    int itemUsedCount = itemUI.GetItemAction().Action(itemInstance);
                    _itemRouterService.RemoveItem(_inventoryLogic, itemInstance, itemUsedCount);
                    break;
                default:
                    Debug.Log("This item not usable");
                    break;
            }
        }
        
        public void OpenInventoryPanel()
        {
            if (!gameObject.activeInHierarchy)
            {
                _panelController.UpdatePanel(itemPanelSystem);
                inventoryObject.SetActive(true);
            }
            else
            {
                inventoryObject.SetActive(false);
            }
        }
    }
}