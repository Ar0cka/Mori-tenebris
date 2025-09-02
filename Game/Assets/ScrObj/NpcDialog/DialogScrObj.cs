using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC.DialogSystem
{
    [CreateAssetMenu(fileName = "npcReputationStats", menuName = "NPC/ReputationStats", order = 0)]
    public class DialogScrObj : ScriptableObject
    {
        [SerializeField] private NpcReputatuionStats npcStats;

        public NpcReputatuionStats GetCopyStats()
        {
            return npcStats.GetCopyStats();
        }
    }
    
    [Serializable]
    public class NpcReputatuionStats
    {
        [field:SerializeField] public int reputation;
        [field:SerializeField] public int gold;
        [field:SerializeField] public bool questCompleted;
        [field:SerializeField] public bool questCompleting;

        public void ChangeReputation(int count, Operations operation)
        {
            switch (operation)
            {
                case Operations.Mines:
                    reputation -= count;
                    break;
                case Operations.Plus:
                    reputation += count;
                    break;
                case Operations.Initialize:
                    reputation = 0;
                    break;
            }
        }

        public void TakeQuest() => questCompleting = true;

        public void CompleteQuest()
        {
            questCompleted = true;
            questCompleting = false;
        }

        public NpcReputatuionStats GetCopyStats()
        {
            return MemberwiseClone() as NpcReputatuionStats;
        }
    }
    public enum Operations
    {
        Mines, 
        Plus,
        Initialize
    }
}