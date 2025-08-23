using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ShopPanel
{
    public class ShopItemPanel : ItemPanelSystem
    {
        [SerializeField] private ShopPanel shopPanel;
        [SerializeField] private Button buyButton;
        [SerializeField] private Image image;

        public override void Open(ItemUI itemUI)
        {
            base.Open(itemUI);
            image.sprite = itemUI.GetImage();
            buyButton.onClick.AddListener(PanelAction);
        }
        
        protected override void PanelAction()
        {
            if (CurrentItem != null)
            {
                shopPanel.RouteItem(CurrentItem.GetCurrentInventory(), CurrentItem.GetItemInstance(), 1);
                Close();
            }
        }
        
        public override void Close()
        {
            base.Close();
            buyButton.onClick.RemoveListener(PanelAction);
        }
    }
}