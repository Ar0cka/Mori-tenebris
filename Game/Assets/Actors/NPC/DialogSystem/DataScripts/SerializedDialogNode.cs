using System;
using System.Collections.Generic;
using Actors.NPC.NpcStateSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC.DialogSystem.DataScripts
{
    [Serializable]
    public class SerializedDialogNode
    {
        public string id;
        public bool isStartNode;
        public List<DialogData> npcDialog;
        public DialogData playerDialog;
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
    }

    [Serializable]
    public class DialogSpecialPanelSettings
    {
        public SpecialPanelType specialPanelType;
    }
}