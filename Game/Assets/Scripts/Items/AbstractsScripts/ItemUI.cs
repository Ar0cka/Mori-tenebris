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

        private ItemInstance _itemInstance;
        private AbstractInventoryLogic _currentInventory;

        private Action _customListener;
        private Action<object> _customListenerWithContext;
        private Action _defaultListener;
        
        public void InitializeItemSettings(ItemInstance itemInstance, AbstractInventoryLogic inventoryLogic)
        {
            _itemInstance = itemInstance;
            image.sprite = _itemInstance.itemData.iconItem;

            _currentInventory = inventoryLogic;

            _defaultListener = UiAction;
            itemButton.onClick.AddListener(() => _defaultListener?.Invoke());
        }
        
        public ItemInstance GetItemInstance() => _itemInstance;
        public AbstractInventoryLogic GetCurrentInventory() => _currentInventory;
        public ItemAction GetItemAction() => itemAction;
        public virtual void UiAction()
        {
            _panelController.OpenPanel(this);
        }
        
        public void UpdateUI(int amount)
        {
            countUI.text = amount.ToString();
        }
        
        public Sprite GetImage() => image.sprite;
        
        
        public void CustomListener(Action clickAction)
        {
            itemButton.onClick.RemoveAllListeners();
            _customListener = clickAction;
            
            itemButton.onClick.AddListener(() => _customListener?.Invoke());
        }

        public void CustomListener(Action<object> clickAction, object ctx = null)
        {
            itemButton.onClick.RemoveAllListeners();
            _customListenerWithContext  = clickAction;
            
            itemButton.onClick.AddListener(() => _customListenerWithContext?.Invoke(ctx));
        }
        
        public void DefaultListener()
        {
            itemButton.onClick.RemoveAllListeners();
            _customListener = null;
            itemButton.onClick.AddListener(() => _defaultListener?.Invoke());
        }
        
        public void DeleteObjectFromSlot()
        {
            itemButton.onClick.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}
