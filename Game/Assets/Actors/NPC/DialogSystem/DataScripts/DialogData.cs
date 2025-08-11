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
        public DialogNode(DialogData playerDialogData, List<DialogData> npcDialogData, DialogCondition condition, DialogSpecialPanelSettings dialogSpecialPanel)
        {
            NpcDialogData = npcDialogData;
            PlayerDialogData = playerDialogData;
            Condition = condition;
            SpecialPanelSettings = dialogSpecialPanel;
            
            _childreNodes = new List<DialogNode>();
        }
        
        public List<DialogData> NpcDialogData { get; private set; }
        public bool IsStart { get; private set; }
        public DialogData PlayerDialogData { get; private set; }
        public DialogCondition Condition { get; private set; }
        public DialogSpecialPanelSettings SpecialPanelSettings { get; private set; }

        private List<DialogNode> _childreNodes;

        public void AddNewChild(DialogNode node)
        {
            _childreNodes.Add(node);
        }

        public void MakAtStartNode()
        {
            IsStart = true;
        }

        public List<DialogNode> GetNextNodes()
        {
            if (_childreNodes.Count == 0) return null;
            
            return _childreNodes;
        }
    }
}