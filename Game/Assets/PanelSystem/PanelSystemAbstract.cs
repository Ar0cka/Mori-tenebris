using Enemy;
using Player.Inventory;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PanelSystemAbstract : MonoBehaviour, IPanelOpen
    {
        [SerializeField] protected GameObject panelObject;
        [SerializeField] protected TextMeshProUGUI itemNameText;
        [SerializeField] protected TextMeshProUGUI itemDescriptionText;
        [SerializeField] protected TextMeshProUGUI itemCountText;

        protected ItemInstance CurrentItem;
        
        public virtual void Open(ItemInstance item)
        {
            if (item == null) return;
            
            CurrentItem = item;
            panelObject.SetActive(true);
            UpdatePanelText();
        }

        protected virtual void UpdatePanelText()
        {
            ItemData itemData = CurrentItem.itemData;

            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
            itemCountText.text = CurrentItem.amount.ToString();
        }

        protected abstract void PanelAction();
        
        public void Close()
        {
            panelObject.SetActive(false);
        }
    }
}