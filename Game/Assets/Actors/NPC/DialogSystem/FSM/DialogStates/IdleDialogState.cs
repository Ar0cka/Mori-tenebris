using System;
using Actors.NPC.DialogSystem.DataScripts;

namespace Actors.NPC.DialogSystem.DialogStates
{
    public class IdleDialogState : DialogState
    {
        private Action<DialogNode> _onSelectedData;
        
        public IdleDialogState(DialogFSM stateMachine) : base(stateMachine)
        {
            Fsm = stateMachine;
        }

        public void EnterToIdle(Action<DialogNode> callback)
        {
            _onSelectedData = callback;
            _onSelectedData += ChangeRunningState;
        }

        public override void FixedUpdate()
        {
            if (IsEnterToState)
            {
                Fsm.ChangeState<PlayerDialogState>(CurrentDialogNode);
            }
        }

        private void ChangeRunningState(DialogNode dialogData)
        {
            IsEnterToState = true;
            CurrentDialogNode = dialogData;
        }
    }
}