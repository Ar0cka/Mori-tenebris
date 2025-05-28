using System;
using Player.Inventory.InventoryInterface;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class TakeItems : MonoBehaviour
    {
        [SerializeField] private GameObject putText;
        [SerializeField] private ItemScrObj itemScrObj;

        private ItemInstance _itemInstance;
        private int _countAdd;

        private bool _initialized;

        public void Initialize(int countItem)
        {
            _itemInstance = new ItemInstance(itemScrObj.GetItemData());
            _countAdd = countItem;
            _initialized = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                putText.SetActive(true);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_initialized) return;
            
            Debug.Log("OnTriggerStay2D");
            
            if (other.CompareTag("Player"))
            {
                Debug.Log("See player");
                
                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("Put E");
                    other.gameObject.GetComponent<TakeItemInInventory>().TakeItem(_itemInstance, _countAdd);
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                putText.SetActive(false);
            }
        }
    }
}