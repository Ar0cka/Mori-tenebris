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
    
    public class DialogNode
    {
        public DialogData NpcDialogData { get; private set; }
        public DialogData PlayerDialogData { get; private set; }
        public DialogCondition Condition { get; private set; }
        public DialogSpecialPanelSettings SpecialPanelSettings { get; private set; }

        private List<DialogNode> _childreNodes;
    }

    [Serializable]
    public class DialogNodeForGraph
    {
        public string guid;
        public DialogData npcData;
        public DialogData playerData;
        public DialogCondition condition;
        public DialogSpecialPanelSettings panelSettings;
        public Vector2 position;
        public List<string> childrenGuids;
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