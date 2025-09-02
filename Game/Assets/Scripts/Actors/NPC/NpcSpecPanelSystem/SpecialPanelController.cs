using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem;
using Actors.NPC.SpecialPanel;
using Actors.Player.Inventory;
using ConsoleApp.Runtime;
using DefaultNamespace;
using DefaultNamespace.ShopPanel;
using EconomicSystem;
using Player.Inventory;
using UnityEngine;

namespace Actors.NPC.NpcSpecPanelSystem
{
    public class SpecialPanelController : MonoBehaviour
    {
        [SerializeField] private List<SpecialPanelContext> specialPanelContexts;
        
        private SpecialPanelRegister _specialPanelRegister;
        private DialogFSM _dialogFsm;

        public void Initialize(DialogFSM dialogFsm)
        {
            if (dialogFsm == null)
            {
                ConsoleLogger.Error("DialogFsm is null in class SpecialPanelController");
                return;
            }
            
            _dialogFsm = dialogFsm;
            _dialogFsm.OnOpenShop += ShowShopPanel;
            
            _specialPanelRegister = new SpecialPanelRegister();
            
            foreach (var panel in specialPanelContexts)
            {
                _specialPanelRegister.Register(panel.type, panel.basePanel);
            }
        }

        private void ShowShopPanel(SpecialPanelType specialPanelType, InventoryPanel inventoryLogic, IWallet targetWallet)
        {
            var shop = _specialPanelRegister.GetObject(specialPanelType);

            if (shop is ShopPanel shopPanel)
            {
                shopPanel.SendShopContext(inventoryLogic, targetWallet);
            }
        }
    }

    [Serializable]
    public class SpecialPanelContext
    {
        public SpecialPanelType type;
        public BasePanel basePanel;
    }
}