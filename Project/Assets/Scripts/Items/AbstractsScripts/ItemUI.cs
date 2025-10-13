using System;
using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using DefaultNamespace;
using Enemy;
using JetBrains.Annotations;
using Player.Inventory.InventoryInterface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Object = System.Object;

namespace Player.Inventory
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countUI;
        [SerializeField] private ItemAction itemAction;

        [SerializeField] private Button itemButton;
        
        [Inject] private PanelController _panelController;
        
        private Func<object> _onClick;
        
        private ItemInstance _itemInstance;
        private AbstractInventoryLogic _currentInventory;
        
        public void InitializeItemSettings(ItemInstance itemInstance, AbstractInventoryLogic inventoryLogic)
        {
            _itemInstance = itemInstance;
            image.sprite = _itemInstance.itemData.iconItem;

            _currentInventory = inventoryLogic;
            
            DefaultListener();
            
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(OnClick);
        }
        
        public ItemInstance GetItemInstance() => _itemInstance;
        public AbstractInventoryLogic GetCurrentInventory() => _currentInventory;
        public ItemAction GetItemAction() => itemAction;
        
        public void CountUIUpdate(int amount)
        {
            countUI.text = amount.ToString();
        }
        
        public Sprite GetImage() => image.sprite;
        
        #region Listeners

        protected virtual void OnClick()
        {
            _onClick?.Invoke();
        }
        
        public void CustomListener(Func<object> onClick)
        {
            _onClick = onClick;
        }
        
        public void DefaultListener()
        {
            _onClick = () =>
            {
                _panelController.OpenPanel(this);
                return null;
            };
        }

        public void RemoveListener()
        {
            _onClick = null;
        }

        #endregion

        #region DeinstalItem

        public void ClearItem()
        {
            _itemInstance = null;
            image.sprite = null;
            countUI.text = "";

            _currentInventory = null;
            itemButton.onClick.RemoveAllListeners();
        }
        
        public void DeleteObjectFromSlot()
        {
            itemButton.onClick.RemoveAllListeners();
            Destroy(gameObject);
        }

        #endregion
        
    }
}
