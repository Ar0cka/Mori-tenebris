using Enemy;
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

        public void InitializeCraftItemPanel()
        {
            Debug.Assert(slotParent != null, "slotParent is null");
            _craftPanelInventory = _classFactory.Create<CraftPanelInventory>(craftSettings, slotParent, previewPrefab);
        }
        
        public void Open(RecipesConfig itemRecipesConfig)
        {
            panelObject.SetActive(true); 
            _craftPanelInventory.OpenCraft(itemRecipesConfig.Recipes);
            
            ItemData itemData = itemRecipesConfig.GetResultItemData();
            itemIcon.sprite = itemData.iconItem;
            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
        }

        protected override void PanelAction()
        {
            //В будущем добавить логика обработки всех этапов крафта.
        }
    }
    
    
}