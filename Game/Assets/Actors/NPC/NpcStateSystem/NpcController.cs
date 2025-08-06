using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.NpcStateSystem;
using Actors.NPC.NpcTools;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private NpcContext npcContext;
        [SerializeField] private NPCAbstractSpecialPanel specialPanel;

        private NpcRepSystem _npcRepSystem;
        
        public void InitializeNpcSystems()
        {
            _npcRepSystem = new NpcRepSystem();
            _npcRepSystem.InitializeStats(npcContext.ReputationData); 
        }
        
        public IReputationOperations GetNpcRepSystem() => _npcRepSystem;
        public INpcSpecialPanel GetSpecialPanel() => specialPanel;
    }
}