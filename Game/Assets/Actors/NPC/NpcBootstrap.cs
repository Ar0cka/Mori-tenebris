using System;
using System.Linq;
using System.Reflection;
using Actors.NPC.DialogSystem;
using UnityEngine;

namespace Actors.NPC
{
    public class NpcBootstrap : MonoBehaviour
    {
        [SerializeField] private NpcController npcController;

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