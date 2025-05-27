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
        [SerializeField] private ItemScrObj itemScrObj;

        private ItemInstance _itemInstance;
        public bool _IsEquiped;
        
        public void InitializeItemSettings()
        {
            if (itemScrObj == null)
            {
                DeleteObjectFromSlot();
                return;
            }
            
            _itemInstance = new ItemInstance(itemScrObj.GetItemData());

            image.sprite = _itemInstance.itemData.iconItem;
        }

        public ItemInstance PutItem()
        {
            if (_itemInstance == null) return null;

            return _itemInstance;
        }
        
        public void ActiveItem()
        {
            if (itemAction == null || _itemInstance == null) return; 
            
            ItemPanelInstance.OpenPanel(_itemInstance, itemAction, _IsEquiped);
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
