using Data;
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
        
        [Inject] private IFindItemInInventory _findItem;
        [Inject] private IAddNewItemOnInventory _addNewItem;
        
        private ItemData itemData;
        
        private void Awake()
        {
            itemData = itemScrObj.ItemData;
            image.sprite = itemData.iconItem;
        }

        public void UseItem()
        {
            Debug.Log("Item name = " + itemData.nameItem);

            if (_findItem == null)
            {
                Debug.LogError("No find item found");
                return;
            }
            
            var slot = _findItem.FindItemInInventory(itemData.nameItem);

            if (slot != null)
            {
                itemAction.ActionItem(itemScrObj, slot);
            }
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