using Actors.Player.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CraftPanel : BasePanel
    {
        [SerializeField] private CraftItemPanel craftItemPanel;
        
        [Inject] private PanelController _panelController;

        private CraftContext _craftContext;
        
        public void InitializeCraftPanel()
        {
            craftItemPanel.InitializeCraftItemPanel();
        }

        public void Open()
        {
            OpenPanel();
        }
        
        public override void OpenPanel(PanelContext panelContext = null)
        {
            inventoryObject.SetActive(true);
            _panelController.UpdatePanel<RecipesConfig>(craftItemPanel);
            
            _craftContext = (CraftContext)panelContext;
            
            if (_craftContext == null) return;
        }
    }

    public class CraftContext : PanelContext
    {
        public AbstractInventoryLogic PlayerInventory;

        public CraftContext(AbstractInventoryLogic playerInventory)
        {
            PlayerInventory = playerInventory;
        }
    }
}