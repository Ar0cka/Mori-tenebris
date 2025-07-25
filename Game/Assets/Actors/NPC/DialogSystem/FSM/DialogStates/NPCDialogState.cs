using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;

namespace Actors.NPC.DialogSystem
{
    public class NPCDialogState : DialogState
    {
        private Action OnClick;
        
        public NPCDialogState(DialogFSM fsm, Action<string> sendTextDatAction, Action onClick) : base(fsm)
        {
            Fsm = fsm;
            SendDialogData = sendTextDatAction;
            OnClick = onClick;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            
            DialogTimeCode = node.npcDialogData.timeCode;
            
            SendDialogEvent(node.npcDialogData.text);
            OnClick += MouseClicked;
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
            OnClick -= MouseClicked;
        }
    }
}