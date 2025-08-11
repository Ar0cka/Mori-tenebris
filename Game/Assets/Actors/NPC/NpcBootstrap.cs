using System;
using System.Linq;
using System.Reflection;
using Actors.NPC.DialogSystem;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC
{
    public class NpcBootstrap : MonoBehaviour
    {
        [SerializeField] private NpcController npcController;
        [SerializeField] private DialogFsmRealize dialogFsmRealize;
        [SerializeField] private TestDialogUI testDialogUI; //TO DO: Заменить на конкертную реализацию диалоговой панели (она будет общая)
        [FormerlySerializedAs("startDialogNodeScrObj")] [SerializeField] private DialogGraphAsset startDialogGraphAsset;

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
            dialogFsmRealize.Initialize();
            testDialogUI.Initialize(dialogFsmRealize.GetDialogFsm(), startDialogGraphAsset);
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