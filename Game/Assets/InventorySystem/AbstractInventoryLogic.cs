using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using SlotSystem;
using UnityEngine;
using Zenject;

namespace Actors.Player.Inventory
{
    public abstract class AbstractInventoryLogic: IInventoryAdder, IInventoryRemove 
    {
        protected ISpawnProjectObject ItemFactory;
        
        protected InventoryScrObj InventoryScrObj;
        protected GameObject SlotPrefab;
        protected Transform SlotParent;
        protected int CapacityInventory;
        
        protected List<SlotData> Slots = new List<SlotData>();
        protected Dictionary<ItemInstance, SlotData> SlotData = new();
        protected Stack<SlotData> SlotStack = new();
        protected bool BaseFunctionInit = false;

        [Inject]
        public AbstractInventoryLogic(ISpawnProjectObject itemFactory)
        {
            ItemFactory = itemFactory;

            if (ItemFactory == null)
            {
                Debug.LogError("Can't find ItemFactory");
            }
        }
        
        public abstract void Initialize<TConfig>(TConfig baseInventoryConfiguration)
            where TConfig : BaseInventoryInitializeConfiguration;
        
        protected virtual void BaseInit(Transform slotParent, InventoryScrObj inventoryScrObj)
        {
            if (ItemFactory == null)
            {
                Debug.LogError("No ItemFactory provided");
                return;
            }
            
            #region InitializeInventory

            InventoryScrObj = inventoryScrObj;

            var inventoryData = InventoryScrObj.InventoryData.Clone();

            SlotPrefab = inventoryData.SlotPrefab;
            SlotParent = slotParent;
            CapacityInventory = inventoryData.CountSlots;

            #endregion

            for (int i = 0; i < CapacityInventory; i++)
            {
                var prefabSlot = ItemFactory.Create(SlotPrefab, SlotParent);
                var slotData = new SlotData(prefabSlot, ItemFactory, this);
                Slots.Add(slotData);
            }

            for (int i = Slots.Count - 1; i >= 0; i--)
            {
                var slot = Slots[i];
                SlotStack.Push(slot);
            }

            BaseFunctionInit = true;
        }
        
        public virtual void AddItemToInventory(ItemInstance itemInstance, int amount)
        {
            if (amount <= 0 || itemInstance == null)
            {
                Debug.Log("item data = " + itemInstance + " amount add " + amount);
                return;
            }

            int remainingAmount = amount;

            remainingAmount = AddItem(itemInstance, remainingAmount);

            if (remainingAmount > 0)
            {
                remainingAmount = CreateNewStacks(itemInstance, remainingAmount);
            }

            if (remainingAmount > 0 && SlotStack.Count == 0)
            {
                Debug.Log($"Inventory full. Couldn't add {remainingAmount} of {itemInstance.itemData.nameItem}");
            }
            else
            {
                AddItemToInventory(itemInstance, remainingAmount);
            }
        }
        
        protected virtual int AddItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in Slots)
            {
                if (remaining <= 0) break;

                remaining = slot.AddItem(itemInstance, remaining);
            }

            return remaining;
        }
        
        protected virtual int CreateNewStacks(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            if (remaining <= 0) return remaining;
            
            var slot = SlotStack.Pop();

            slot.CreateNewItem(itemInstance);
            remaining = slot.AddItem(itemInstance, remaining);

            return remaining;
        }

        public virtual void RemoveItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in Slots)
            {
                if (remaining <= 0)
                {
                    Debug.Log("Break");
                    break;
                }

                remaining = slot.RemoveItem(itemInstance, remaining);
                
                if (slot.IsEmpty())
                {
                    SlotStack.Push(slot);
                }
            }

            if (remaining > 0)
            {
                Debug.Log($"Cant find item in inventory. Couldn't remove item {remaining}");
            }
        }

        public SlotData GetSlotFromInventory(ItemInstance itemInstance)
        {
            var item = FindItem(itemInstance);
            
            if (item == null) return null;
            
            SlotStack.Push(item);
            return item;
        }
        
        public virtual SlotData FindItem(ItemInstance itemInstance)
        {
            foreach (var slot in Slots)
            {
                if (slot.CheckItemInSlot(itemInstance.itemID))
                {
                    return slot;
                }
            }

            return null;
        }

        public SlotData GetFirstFreeSlot()
        {
            return SlotStack.Pop();
        }
    }
}