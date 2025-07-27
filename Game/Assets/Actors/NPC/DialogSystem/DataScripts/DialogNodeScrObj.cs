using UnityEngine;

namespace Actors.NPC.DialogSystem.DataScripts
{
    [CreateAssetMenu(fileName = "DialogScr", menuName = "Npc/DialogScr", order = 0)]
    public class DialogNodeScrObj : ScriptableObject
    {
        [SerializeField] private DialogNode dialogNode;
        
        public DialogNode GetCurrentDialogNode() => dialogNode;
    }
}