using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ShopPanel
{
    public class ShopItemPanel : ItemPanelSystem
    {
        [SerializeField] private ShopPanel shopPanel;
        [SerializeField] private Button buyButton;
        
        public virtual void Open(ItemUI itemUI)
        {
            base.Open(itemUI);
            buyButton.onClick.AddListener(PanelAction);
        }

        protected override void PanelAction()
        {
            if (CurrentItem != null)
            {
                shopPanel.RouteItem(CurrentItem.GetCurrentInventory(), CurrentItem.GetItemInstance(), 1);
            }
        }
        
        public virtual void Close()
        {
            base.Close();
        }
    }
}