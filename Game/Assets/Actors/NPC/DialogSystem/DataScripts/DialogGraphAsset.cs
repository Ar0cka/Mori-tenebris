using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Actors.NPC.DialogSystem.DataScripts
{
    [CreateAssetMenu(fileName = "DialogScr", menuName = "Npc/DialogScr", order = 0)]
    public class DialogGraphAsset : ScriptableObject
    {
        public List<SerializedDialogNode> dialogNode;
    }
}