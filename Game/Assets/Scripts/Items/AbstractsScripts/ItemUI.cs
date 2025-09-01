using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using DefaultNamespace;
using Enemy;
using Player.Inventory.InventoryInterface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace Player.Inventory
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countUI;
        [SerializeField] private ItemAction itemAction;

        [Inject] private PanelController _panelController;

        private ItemInstance _itemInstance;
        private AbstractInventoryLogic _currentInventory;
        
        public void InitializeItemSettings(ItemInstance itemInstance, AbstractInventoryLogic inventoryLogic)
        {
            _itemInstance = itemInstance;
            image.sprite = _itemInstance.itemData.iconItem;

            _currentInventory = inventoryLogic;
        }
        
        public ItemInstance GetItemInstance() => _itemInstance;
        public AbstractInventoryLogic GetCurrentInventory() => _currentInventory;
        public ItemAction GetItemAction() => itemAction;
        public void UiAction()
        {
            _panelController.OpenPanel(this);
        }
        
        public void UpdateUI()
        {
            countUI.text = _itemInstance.amount.ToString();
        }
        
        public Sprite GetImage() => image.sprite;
        
        public void DeleteObjectFromSlot()
        {
            Destroy(gameObject);
        }
    }
}
