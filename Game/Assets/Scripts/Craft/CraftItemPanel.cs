using System;
using Enemy;
using JetBrains.Annotations;
using Player.Inventory;
using Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class CraftItemPanel : ItemPanelSystem, IPanelOpen<RecipesConfig>
    {
        [Header("Data")]
        [SerializeField] private InventoryScrObj craftSettings;

        [Header("Components")] 
        [SerializeField] private Transform slotParent;
        [SerializeField] private GameObject previewPrefab;
        
        [Header("Craft UI")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private Button craftButton;
        
        [Inject] private ZenjectClassFactory _classFactory;
        
        private CraftPanelInventory _craftPanelInventory;
        
        private RecipesConfig _config;

        public void InitializeCraftItemPanel()
        {
            Debug.Assert(slotParent != null, "slotParent is null");
            _craftPanelInventory = _classFactory.Create<CraftPanelInventory>(craftSettings, slotParent, previewPrefab);
        }
        
        public void Open(RecipesConfig itemRecipesConfig)
        {
            _config = itemRecipesConfig;
            
            panelObject.SetActive(true); 
            _craftPanelInventory.OpenCraft(itemRecipesConfig.Recipes);
            
            ItemData itemData = itemRecipesConfig.GetResultItemData();
            itemIcon.sprite = itemData.iconItem;
            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
            
            craftButton.onClick.AddListener(PanelAction);
        }

        protected override void PanelAction()
        {
            //В будущем добавить логика обработки всех этапов крафта.
        }

        public void RegisterNewListenerOnItem(ItemUI itemUI)
        {
            itemUI.CustomListener(() => _craftPanelInventory.AddItemOnCraftSlot(itemUI));
        }
        
        public override void Close()
        {
            _craftPanelInventory.CraftPanelClose();
            base.Close();
        }
    }
    
    
}