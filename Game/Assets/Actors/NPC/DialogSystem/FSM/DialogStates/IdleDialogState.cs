using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;

namespace Actors.NPC.DialogSystem.DialogStates
{
    public class IdleDialogState : DialogState
    {
        public IdleDialogState(DialogFSM stateMachine) : base(stateMachine)
        {
            Fsm = stateMachine;
        }
        
        public void EnterToIdle()
        {
            Fsm.OnStartDialog += ChangeRunningState;
        }

        public override void Enter(DialogNode node)
        {
            EnterToIdle();
        }

        public override void Update()
        {
           
        }

        private void ChangeRunningState(DialogNode dialogData)
        {
            CurrentDialogNode = dialogData;

            if (CurrentDialogNode == null)
            {
                Debug.Log("Current dialog node is null");
            }
            
            Fsm.ChangeState<PlayerDialogState>(CurrentDialogNode);
        }

        public override void Exit()
        {
            base.Exit();
            Fsm.OnStartDialog -= ChangeRunningState;
            Fsm.ExitFromIdleState();
        }
    }
}