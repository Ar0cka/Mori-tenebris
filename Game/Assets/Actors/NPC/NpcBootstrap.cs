using System;
using System.Linq;
using System.Reflection;
using Actors.NPC.DialogSystem;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.TestUI;
using Actors.NPC.Inventory;
using Actors.NPC.NpcSpecPanelSystem;
using DefaultNamespace.ShopPanel;
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
        
        [Inject] private DiContainer _diContainer;
        
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
            npcController.InitializeNpcSystems();
            
            DialogInit();
            SpecialPanelInit();
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
            shopPanel.Initialize(npcInventoryPanel);
            specialPanelController.Initialize(dialogFsmRealize.GetDialogFsm());
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