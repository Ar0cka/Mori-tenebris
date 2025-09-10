using DefaultNamespace.ShopPanel;
using Project.Service;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CraftSlots : SlotUI
    {
        private GameObject _previewPrefab;

        public CraftSlots(GameObject slotPrefab, IDestroyService destroyService, GameObject previewPrefab) : base(
            slotPrefab, destroyService)
        {
            _previewPrefab = previewPrefab;
        }

        public void ActivitySlot(bool activeStatus, Sprite previewSprite)
        {
            _slotPrefab.SetActive(activeStatus);
            
            Image previewImage = _previewPrefab.GetComponent<Image>();
            previewImage.sprite = previewSprite;
            
            _previewPrefab.SetActive(activeStatus); 
        }
    }
}