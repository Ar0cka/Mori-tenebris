using Enemy;
using Player.Inventory.InventoryInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
            itemData = itemScrObj.ItemData;
            image.sprite = itemData.iconItem;
        }

        public void UseItem()
        {
            itemAction.ActionItem(itemScrObj);
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
