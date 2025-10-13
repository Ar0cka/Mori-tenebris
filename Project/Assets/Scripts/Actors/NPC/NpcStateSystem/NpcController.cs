using Actors.NPC.NpcStateSystem;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private NpcContext npcContext;

        private NpcRepSystem _npcRepSystem;
        
        public void InitializeNpcSystems()
        {
            _npcRepSystem = new NpcRepSystem();
            _npcRepSystem.InitializeStats(npcContext.ReputationData); 
        }
        
        public IReputationOperations GetNpcRepSystem() => _npcRepSystem;
    }
}