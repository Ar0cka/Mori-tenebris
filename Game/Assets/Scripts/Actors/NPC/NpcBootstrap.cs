using System;
using System.Linq;
using System.Reflection;
using Actors.NPC.DialogSystem;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.TestUI;
using Actors.NPC.Inventory;
using Actors.NPC.NpcSpecPanelSystem;
using DefaultNamespace;
using DefaultNamespace.ShopPanel;
using EconomicSystem;
using Service;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Actors.NPC
{
    public class NpcBootstrap : MonoBehaviour
    {
        [Header("Npc")]
        [SerializeField] private NpcController npcController;
        [SerializeField] private DialogTriggerController dialogTriggerController;
        
        [Header("Npc dialog system")]
        [SerializeField] private DialogFsmRealize dialogFsmRealize;
        [SerializeField] private TestDialogUI testDialogUI; //TO DO: Заменить на конкертную реализацию диалоговой панели (она будет общая)
        [SerializeField] private DialogGraphAsset startDialogGraphAsset;
        
        [Header("Npc Special panels")]
        [SerializeField] private SpecialPanelController specialPanelController;
        [SerializeField] private NpcInventoryPanel npcInventoryPanel;
        [SerializeField] private ShopPanel shopPanel;
        
        [Header("Craft")]
        [SerializeField] private CraftPanel craftPanel;
        
        [Header("Npc economic")]
        [SerializeField] private WalletRealize walletRealize;
        
        [Inject] private DiContainer _diContainer;
        [Inject] private PanelController _panelController;
        
        private void Awake()
        {
            if (!CheckValidity())
            {
                Debug.Log("Problem with initialize script NpcBootstrap");
                enabled = false;
            }
            
            Init();
        }

        private void Init()
        {
            var initRegisterPanelType = new RegisterPanelValuesTypes();
            initRegisterPanelType.InitPanelsValues(_panelController);
            
            npcController.InitializeNpcSystems();
            
            DialogInit();
            EconomicSystem();
            SpecialPanelInit();
            CraftSystem();
        }

        private void DialogInit()
        {
            dialogFsmRealize.Initialize();
            testDialogUI.Initialize(dialogFsmRealize.GetDialogFsm(), startDialogGraphAsset);
            dialogTriggerController.Initialize(dialogFsmRealize.GetDialogFsm());
        }
        
        private void SpecialPanelInit()
        {
            npcInventoryPanel.Initialize();
            shopPanel.Initialize(npcInventoryPanel, dialogFsmRealize.GetDialogFsm());
            specialPanelController.Initialize(dialogFsmRealize.GetDialogFsm());
        }

        private void EconomicSystem()
        {
            walletRealize.Initialize();
        }

        private void CraftSystem()
        {
            craftPanel.InitializeCraftPanel();
        }
        
        private bool CheckValidity()
        {
            bool isValid = true;
            
            var requiredComponents =
                GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in requiredComponents)
            {
                if (field.GetValue(this) == null)
                {
                    Debug.Log(field.Name + " is required == null.");
                    isValid = false;
                }
            }
            
            return isValid;
        }
    }
}