using System;
using DefaultNamespace.Enums;
using Enemy;
using Items.EquipArmour;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using PlayerNameSpace.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Actors.Player.Inventory.Scripts.ItemPanel
{
    public class PanelSettings : MonoBehaviour, IOpenPanel
    {
        [SerializeField] private GameObject itemPanel;
        [SerializeField] private GameObject removeSliderPanel;

        [SerializeField] private Button closeButton;
        [SerializeField] private Button useButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private Button deleteItems;
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private Slider slider;

        [Header("Text")] 
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemRemoveCountText;
        [SerializeField] private TextMeshProUGUI itemCountText;

        [Inject] private InventoryLogic inventoryLogic;

        private ItemInstance _currentItemInstance;
        private ItemAction _currentAction;

        private bool _isEquiped;

        private void Awake()
        {
            ItemPanelInstance.Initialize(this);
            gameObject.SetActive(false);
            closeButton.onClick.AddListener(ClosePanel);
            useButton.onClick.AddListener(UseItem);
            removeButton.onClick.AddListener(RemoveItem);
            deleteItems.onClick.AddListener(AcceptRemoveItem);
            cancelButton.onClick.AddListener(CancelRemoveItem);
            
            slider.wholeNumbers = true;

            itemPanel.SetActive(false);
            removeSliderPanel.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (itemPanel.activeInHierarchy)
            {
                itemRemoveCountText.text = slider.value.ToString();
            }
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
            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
            itemCountText.text = $"Current count = {_currentItemInstance.amount}";
        }

        private void UseItem()
        {
            if (_currentAction != null && _currentItemInstance != null)
            {
                if (_currentItemInstance.itemData.itemTypes == ItemTypes.Equip)
                {
                    if (_currentAction is EquipAction equipAction)
                    {
                        SlotData slot = equipAction.IsEquipped
                            ? inventoryLogic.GetFirstFreeSlot()
                            : inventoryLogic.GetItemFromInventory(_currentItemInstance);
                        
                        equipAction.EquipItem(_currentItemInstance, slot);
                        ClosePanel();
                    }
                }
                else
                {
                    _currentAction.ActionItem(_currentItemInstance);
                }
            }
        }

        private void RemoveItem()
        {
            removeSliderPanel.SetActive(true);
            slider.value = 0;
            if (_currentItemInstance != null)
            {
                slider.minValue = 0;
                slider.maxValue = _currentItemInstance.amount;
            }
        }

        private void CancelRemoveItem()
        {
            removeSliderPanel.SetActive(false);
        }

        private void AcceptRemoveItem()
        {
            if (_currentItemInstance == null) return;
            
            inventoryLogic.RemoveItem(_currentItemInstance, (int)slider.value);

            if (_currentItemInstance.amount <= 0)
            {
                CancelRemoveItem();
                ClosePanel();
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