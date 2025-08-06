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
        public int timeCode = 3;
        public NpcReputationEnum dialogReputation = NpcReputationEnum.Neutral;
    }
    
    public class DialogNode
    {
        public DialogNode(DialogData playerDialogData, DialogData npcDialogData, DialogCondition condition, DialogSpecialPanelSettings dialogSpecialPanel)
        {
            NpcDialogData = npcDialogData;
            PlayerDialogData = playerDialogData;
            Condition = condition;
            SpecialPanelSettings = dialogSpecialPanel;
            
            _childreNodes = new List<DialogNode>();
        }
        
        public DialogData NpcDialogData { get; private set; }
        public bool IsStart { get; private set; }
        public DialogData PlayerDialogData { get; private set; }
        public DialogCondition Condition { get; private set; }
        public DialogSpecialPanelSettings SpecialPanelSettings { get; private set; }

        private List<DialogNode> _childreNodes;

        public void AddNewChild(DialogNode node)
        {
            _childreNodes.Add(node);
        }

        public void SetStartNode()
        {
            IsStart = true;
        }

        public List<DialogNode> GetNextList() => _childreNodes;
    }

    [Serializable]
    public class DialogNodeForGraph
    {
        public string guid;
        public bool isStart;
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