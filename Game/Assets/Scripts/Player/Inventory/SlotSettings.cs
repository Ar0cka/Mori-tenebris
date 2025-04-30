using System;
using EventBusNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player.Inventory
{
    public class SlotSettings : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countUI;

        private SlotData slotData;
        
        private void Awake()
        {
            EventBus.Subscribe<SendUpdateSlotUI>(e => UpdateUI(e.Count));
        }

        public void AddNewItem(ItemData itemData, SlotData slotData)
        {
            image.sprite = itemData.iconItem;
            this.slotData = slotData;
        }
        
        private void UpdateUI(int countItemsInSlot)
        {
            countUI.text = countItemsInSlot.ToString();
        }
        
        public void DeleteObjectFromSlot()
        {
            image.sprite = null;
            countUI.text = "";
        }

        private void OnApplicationQuit()
        {
            EventBus.Unsubscribe<SendUpdateSlotUI>(e => UpdateUI(e.Count));
        }
    }

    public class SendUpdateSlotUI
    {
        public int Count { get; }
        
        public SendUpdateSlotUI(int count)
        {
            Count = count;
        }
    }
}