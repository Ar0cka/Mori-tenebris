using UnityEngine;

namespace Actors.NPC.NpcStateSystem
{
    [CreateAssetMenu(fileName = "NpcContext", menuName = "NPC/Context", order = 0)]
    public class NpcContext : ScriptableObject
    {
        [SerializeField] private ReputationData reputationData;
        
        public ReputationData ReputationData => reputationData;
    }
}