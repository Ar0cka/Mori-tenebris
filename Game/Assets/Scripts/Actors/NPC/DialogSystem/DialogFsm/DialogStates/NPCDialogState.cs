using System;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.DialogSystem.TestUI;

namespace Actors.NPC.DialogSystem
{
    public class NPCDialogState : DialogState
    {
        private NpcController _npcController;
        
        public NPCDialogState(DialogFSM stateMachine, NpcController npcController) : base(stateMachine)
        {
            Fsm = stateMachine;
            _npcController = npcController;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);

            var npcNode = GetChildrenNode(node);
            
            DialogTimeCode = npcNode.timeCode;
            
            SendDialogEvent(npcNode.text);
            Fsm.OnClick += MouseClicked;
        }

        private DialogData GetChildrenNode(DialogNode node)
        {
            var reputation = _npcController.GetNpcRepSystem();

            foreach (var child in node.NpcDialogData)
            {
                if (child.dialogReputation == reputation.GetCurrentNpcReputationState())
                {
                    return child;
                }
            }

            return node.NpcDialogData.First();
        }
        
        private void MouseClicked()
        {
            if (!IsCompleted)
                Complete();
        }

        protected override void Complete()
        {
            base.Complete();
            bool openPanel = CheckAndSwitchOnPanelState();
            
            if (!openPanel) 
                ChangeDialogState<EndDialogState>();
        }
        
        public override void Exit()
        {
            base.Exit();
            Fsm.OnClick -= MouseClicked;
        }
    }
}