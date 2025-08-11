using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.DialogSystem.TestUI;

namespace Actors.NPC.DialogSystem
{
    public class EndDialogState : DialogState
    {
        public EndDialogState(DialogFSM stateMachine) : base(stateMachine)
        {
            Fsm = stateMachine;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            EndAction();
        }

        private void EndAction()
        {
            SendDialogsNodes();
            ChangeDialogState<IdleDialogState>();
        }
        
        public override void Update()
        {
            // Конечное состояния, отключаем базовый FixedUpdate
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}