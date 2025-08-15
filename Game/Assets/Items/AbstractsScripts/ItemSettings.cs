using Actors.Player.Inventory.Scripts.ItemPanel;
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

        private ItemInstance _itemInstance;
        public bool IsEquiped;
        
        public void InitializeItemSettings(ItemInstance itemInstance)
        {
            _itemInstance = itemInstance;
            image.sprite = _itemInstance.itemData.iconItem;
        }
        
        public void ActiveItem()
        {
            if (itemAction == null || _itemInstance == null) return; 
            
            ItemPanelInstance.OpenPanel(_itemInstance, itemAction, IsEquiped);
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
