using System;
using System.Collections.Generic;
using System.Linq;
using Actors.NPC.NpcStateSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC.DialogSystem.DataScripts
{
    [Serializable]
    public class DialogData
    {
        public string text;
        public int timeCode;
        public NpcReputationEnum dialogReputation;
    }
    
    [Serializable]
    public class DialogNode
    {
        [field:SerializeField] public DialogData NpcDialogData { get; private set; }
        [field:SerializeField] public DialogData PlayerDialogData { get; private set; }
        [field:SerializeField] public DialogCondition Condition { get; private set; }
        [field:SerializeField] public DialogSpecialPanelSettings SpecialPanelSettings { get; private set; }
        [SerializeField] private List<DialogNode> childrenDialogNodes;
        
        public DialogNode GetNextNode(NpcReputationEnum reputationEnum)
        {
            for (int i = 0; i < childrenDialogNodes.Count; i++)
            {
                if (childrenDialogNodes[i].NpcDialogData.dialogReputation == reputationEnum)
                {
                    return childrenDialogNodes[i];
                }
            }
            
            return childrenDialogNodes.First();
        }

        public List<DialogNode> GetNextNodes() => childrenDialogNodes;
    }

    [Serializable]
    public class DialogCondition : IDialogCondition
    {
        public ConditionType CurrentConditionType { get; set; }
        public DialogActionType ActionType { get; set; }
        public int ReputationNum { get; set; }
    }

    [Serializable]
    public class DialogSpecialPanelSettings
    {
        public SpecialPanelType specialPanelType;
    }
}