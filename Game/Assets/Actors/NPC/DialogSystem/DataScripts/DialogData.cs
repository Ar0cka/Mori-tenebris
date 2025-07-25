using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC.DialogSystem.DataScripts
{
    [Serializable]
    public class DialogData
    {
        [field: SerializeField] public string text;
        [field: SerializeField] public int timeCode;
        [field: SerializeField] public NpcReputationState dialogReputation;
    }
    
    [Serializable]
    public class DialogNode
    {
        [field:SerializeField] public DialogData npcDialogData;
        [field:SerializeField] public DialogData playerDialogData;
        [SerializeField] private List<DialogNode> childrenDialogNodes;

        public DialogNode GetNextNode(NpcReputationState reputationState)
        {
            for (int i = 0; i < childrenDialogNodes.Count; i++)
            {
                if (childrenDialogNodes[i].npcDialogData.dialogReputation == reputationState)
                {
                    return childrenDialogNodes[i];
                }
            }
            
            return childrenDialogNodes.First();
        }

        public List<DialogNode> GetNextNodes() => childrenDialogNodes;
    }

    public enum NpcReputationState
    {
        Friendly,
        Aggressive,
        Neutral
    }
}