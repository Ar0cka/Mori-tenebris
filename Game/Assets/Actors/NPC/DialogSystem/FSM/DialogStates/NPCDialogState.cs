using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.DialogSystem.TestUI;

namespace Actors.NPC.DialogSystem
{
    public class NPCDialogState : DialogState
    {
        public NPCDialogState(DialogFSM stateMachine) : base(stateMachine)
        {
            Fsm = stateMachine;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            
            DialogTimeCode = node.npcDialogData.timeCode;
            
            SendDialogEvent(node.npcDialogData.text);
            Fsm.OnClick += MouseClicked;
        }
        
        private void MouseClicked()
        {
            if (!IsCompleted)
                Complete();
        }

        protected override void Complete()
        {
            base.Complete();
            ChangeDialogState<EndDialogState>();
        }
        
        public override void Exit()
        {
            base.Exit();
            Fsm.OnClick -= MouseClicked;
        }
    }
}