using System;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.ShopPanel
{
    public class ShopItemPanel : ItemPanelSystem
    {
        [SerializeField] private ShopPanel shopPanel;
        [SerializeField] private Button buyButton;
        [SerializeField] private Image image;

        [Header("CountItemBuy")]
        [SerializeField] private TMP_InputField countItemBuy;
        [SerializeField] private Button plusButton;
        [SerializeField] private Button minesButton;

        private int _currentBuyCount;
        
        public override void Open(ItemUI itemUI)
        {
            base.Open(itemUI);
            ItemPanelSettings(itemUI);
            countItemBuy.onEndEdit.AddListener(OnEndEdit);
        }
        
        protected override void PanelAction()
        {
            if (CurrentItem != null)
            {
                shopPanel.RouteItem(CurrentItem.GetCurrentInventory(), CurrentItem.GetItemInstance(), _currentBuyCount);
                Close();
            }
        }

        private void OnEndEdit(string text)
        {
            if (int.TryParse(text, out var count))
            {
                _currentBuyCount = count;
                UpdateText();
                Debug.Log("Convert sucsefull");
                return;
            }
            
            Debug.LogError("Problem with convert");
            _currentBuyCount = 0;
            countItemBuy.text = "0";
        }
        
        private void ItemPanelSettings(ItemUI itemUI)
        {
            _currentBuyCount = 0;
            UpdateText();
            
            image.sprite = itemUI.GetImage();
            
            ItemInstance itemInstance = itemUI.GetItemInstance();

            Debug.Log("Item Instance amount = " + itemInstance.amount);
            _currentBuyCount = Math.Clamp(_currentBuyCount, 0, itemInstance.amount);
            
            buyButton.onClick.AddListener(PanelAction);
            plusButton.onClick.AddListener(PlusItem);
            minesButton.onClick.AddListener(MinesItem);
        }

        private void PlusItem()
        {
            _currentBuyCount += 1;
            UpdateText();
        }
        private void MinesItem()
        {
            _currentBuyCount -= 1;
            UpdateText();
        }

        private void UpdateText()
        {
            _currentBuyCount = Math.Clamp(_currentBuyCount, 0, CurrentItem.GetItemInstance().amount);
            countItemBuy.text = _currentBuyCount.ToString();
        }
        
        public override void Close()
        {
            base.Close();
            buyButton.onClick.RemoveListener(PanelAction);
            plusButton.onClick.RemoveListener(PlusItem);
            minesButton.onClick.RemoveListener(MinesItem);
        }
    }
}