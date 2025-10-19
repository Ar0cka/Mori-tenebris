using Actors.Player.Inventory;
using DefaultNamespace;
using DefaultNamespace.Zenject;
using PlayerNameSpace.Inventory;
using Service;
using UnityEngine;
using Zenject;

namespace Actors.NPC.Inventory
{
    public class NpcInventoryPanel : MonoBehaviour
    {
        [SerializeField] private ItemList itemList;
        [SerializeField] private InventoryScrObj inventoryConfig;
        [SerializeField] private Transform slotHolder;
        [SerializeField] private InventoryObjectType objectType;
        
        [Inject] private InventoryFillService _inventoryFillService;
        [Inject] private DiContainer _diContainer;
        
        private AbstractInventoryLogic _inventoryLogic;

        public void Initialize()
        {
            _inventoryLogic = _diContainer.Instantiate<InventoryLogic>();
            _inventoryLogic.Initialize(new InventoryInitializeConfig(slotHolder, inventoryConfig, objectType));
            
            _inventoryFillService.AddItemFromScrObj(_inventoryLogic, itemList.Items);
        }
        
        public AbstractInventoryLogic GetInventoryLogic() => _inventoryLogic;
    }
}