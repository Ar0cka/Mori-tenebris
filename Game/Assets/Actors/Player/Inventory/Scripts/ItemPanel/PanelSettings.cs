using System;
using Enemy;
using Player.Inventory;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Actors.Player.Inventory.Scripts.ItemPanel
{
    public class PanelSettings : MonoBehaviour, IOpenPanel
    {
        [SerializeField] private GameObject itemPanel;
        
        [SerializeField] private Button closeButton;
        [SerializeField] private Button useButton;
        [SerializeField] private Button removeButton;
        
        private ItemScrObj _currentScriptableObject;
        private ItemAction _currentAction;
        private void Awake()
        {
            ItemPanelInstance.Initialize(this);
            gameObject.SetActive(false);
            closeButton.onClick.AddListener(ClosePanel);
            useButton.onClick.AddListener(UseItem);
        }

        public void OpenPanel(ItemScrObj itemScrObj, ItemAction itemAction)
        {
            if (!itemPanel.activeInHierarchy)
            {
                itemPanel.SetActive(true);
            }
            
            _currentScriptableObject = itemScrObj;
            _currentAction = itemAction;
            UpdateItemData(_currentScriptableObject.ItemData);
        }

        private void UpdateItemData(ItemData itemData)
        {
            // Логика обнавление UI
        }
        
        private void UseItem()
        {
            if (_currentAction != null && _currentScriptableObject != null)
            {
                _currentAction.ActionItem(_currentScriptableObject);
            }
        }
        
        private void ClosePanel()
        {
            _currentScriptableObject = null;
            itemPanel.SetActive(false);
        }
    }

    public static class ItemPanelInstance
    {
        private static IOpenPanel _itemPanelOpen;

        public static void Initialize(PanelSettings panelSettings)
        {
            _itemPanelOpen = panelSettings;
        }

        public static void OpenPanel(ItemScrObj itemScrObj, ItemAction itemAction)
        {
            _itemPanelOpen.OpenPanel(itemScrObj, itemAction);
        }
    }
}