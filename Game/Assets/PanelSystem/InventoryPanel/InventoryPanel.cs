using System.Collections.Generic;
using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using DefaultNamespace;
using DefaultNamespace.Enums;
using Enemy;
using Items;
using Items.EquipArmour.Data;
using Items.Potions.Scripts;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using Service;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class InventoryPanel : BasePanel
    {
        [SerializeField] private List<GameObject> equipSlots;
        [SerializeField] private TakeItemInInventory takeItemInInventory;
        
        [SerializeField] private Transform slotContent;
        [SerializeField] private InventoryScrObj inventoryConfig;

        [SerializeField] private ItemList startItemList;
        
        [Inject] private InventoryFillService _inventoryFillService;
        
        private PlayerEquipSystem _equipSystem;

        public void Initialize()
        {
            InventoryLogic = DiContainer.Instantiate<InventoryLogic>();
            _equipSystem = DiContainer.Instantiate<PlayerEquipSystem>();
            
            InventoryLogic.Initialize(new InventoryInitializeConfig(slotContent, inventoryConfig));
            _equipSystem.InitializeEquipSlots(equipSlots);
            takeItemInInventory.Initialize(InventoryLogic);
            
            _inventoryFillService.AddItemFromScrObj(InventoryLogic, startItemList.Items);
        }

        public override void ItemRouter(ItemUI itemUI)
        {
            var itemInstance = itemUI.GetItemInstance();
            
            ItemAction itemAction = itemUI.GetItemAction();

            if (itemAction == null)
            {
                Debug.Log("Not find item action");
                return;
            }
            
            itemUI.GetItemAction().Action(itemInstance, new ItemActionContext(InventoryLogic, _equipSystem, ItemRouterService));
        }
        
        public void OpenInventoryPanel()
        {
            if (!gameObject.activeInHierarchy)
            {
                PanelController.UpdatePanel(itemPanelSystem);
                inventoryObject.SetActive(true);
            }
            else
            {
                inventoryObject.SetActive(false);
            }
        }

        public AbstractInventoryLogic GetInventoryLogic() => InventoryLogic;
    }
}