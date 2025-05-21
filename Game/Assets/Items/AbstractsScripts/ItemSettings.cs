using Actors.Player.Inventory.Scripts.ItemPanel;
using Enemy;
using Player.Inventory.InventoryInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace Player.Inventory
{
    public class ItemSettings : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ItemScrObj itemScrObj;
        [SerializeField] private TextMeshProUGUI countUI;
        [SerializeField] private ItemAction itemAction;
        
        private ItemData itemData;
        
        private void Awake()
        {
            itemData = itemScrObj.GetItemData();

            if (itemData == null)
            {
                DeleteObjectFromSlot();
                return;
            }
            
            image.sprite = itemData.iconItem;
        }

        public void ActiveItem()
        {
            if (itemAction == null || itemScrObj == null) return; 
            
            ItemPanelInstance.OpenPanel(itemScrObj, itemAction);
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
