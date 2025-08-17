using Actors.Player.Inventory;
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
    public class ItemSettings : MonoBehaviour
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
        }

        public ItemInstance GetItemInstance() => _itemInstance;
        public AbstractInventoryLogic GetCurrentInventory() => _currentInventory;

        public void UiAction()
        {
            _panelController.OpenPanel(this);
        }
        
        public void UpdateUI(int countItemsInSlot)
        {
            countUI.text = countItemsInSlot.ToString();
        }
        
        public void DeleteObjectFromSlot()
        {
            Destroy(gameObject);
        }
    }
}
