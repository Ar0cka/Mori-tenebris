using Enemy;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public abstract class ItemPanelSystem : MonoBehaviour, IPanelOpen
    {
        [SerializeField] protected GameObject panelObject;
        [SerializeField] protected TextMeshProUGUI itemNameText;
        [SerializeField] protected TextMeshProUGUI itemDescriptionText;
        [SerializeField] protected TextMeshProUGUI itemCountText;
        
        [SerializeField] private Button closeButton;

        protected ItemUI CurrentItem;
        
        public virtual void Open(ItemUI item)
        {
            if (item == null) return;
            
            CurrentItem = item;
            panelObject.SetActive(true);
            UpdatePanelText();
            
            closeButton.onClick.AddListener(Close);
        }

        protected virtual void UpdatePanelText()
        {
            ItemData itemData = CurrentItem.GetItemInstance().itemData;

            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
            itemCountText.text = CurrentItem.GetItemInstance().amount.ToString();
        }

        protected abstract void PanelAction();
        
        public virtual void Close()
        {
            panelObject.SetActive(false);
            closeButton.onClick.RemoveAllListeners();
        }
    }
}