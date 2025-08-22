using Actors.Player.Inventory;
using Items;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public abstract class BasePanel : MonoBehaviour
    {
        [SerializeField] protected GameObject inventoryObject;
        [SerializeField] protected ItemPanelSystem itemPanelSystem;
        
        protected AbstractInventoryLogic InventoryLogic;
        
        [Inject] protected PanelController PanelController;
        [Inject] protected ItemRouterService ItemRouterService;
        [Inject] protected DiContainer DiContainer;

        public virtual void ItemRouter(ItemUI itemUI)
        {
            
        }

        public virtual void OpenPanel()
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
    }
}