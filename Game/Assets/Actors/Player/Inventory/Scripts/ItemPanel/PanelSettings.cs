using System;
using DefaultNamespace.Enums;
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
        
        private ItemInstance _currentItemInstance;
        private ItemAction _currentAction;
        
        private bool _isEquiped;
        private void Awake()
        {
            ItemPanelInstance.Initialize(this);
            gameObject.SetActive(false);
            closeButton.onClick.AddListener(ClosePanel);
            useButton.onClick.AddListener(UseItem);
        }

        public void OpenPanel(ItemInstance itemInstance, ItemAction itemAction, bool isEquiped)
        {
            if (!itemPanel.activeInHierarchy)
            {
                itemPanel.SetActive(true);
            }
            
            _currentItemInstance = itemInstance;
            _currentAction = itemAction;
            UpdateItemData(_currentItemInstance.itemData);
            _isEquiped = isEquiped;
        }

        private void UpdateItemData(ItemData itemData)
        {
            // Логика обнавление UI
        }
        
        private void UseItem()
        {
            if (_currentAction != null && _currentItemInstance != null)
            {
                if (_currentItemInstance.itemData.itemTypes == ItemTypes.Equip)
                {
                    _currentAction.EquipItem(_currentItemInstance, _isEquiped);
                    ClosePanel();
                }
                else
                {
                    _currentAction.ActionItem(_currentItemInstance);
                }
            }
        }
        
        private void ClosePanel()
        {
            _currentItemInstance = null;
            _currentAction = null;
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

        public static void OpenPanel(ItemInstance itemInstance, ItemAction itemAction, bool isEquiped)
        {
            _itemPanelOpen.OpenPanel(itemInstance, itemAction, isEquiped);
        }
    }
}