using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;

namespace Actors.NPC.DialogSystem.DialogStates
{
    public class PlayerDialogState : DialogState
    {
        public PlayerDialogState(DialogFSM stateMachine) : base(stateMachine)
        {
            Fsm = stateMachine;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);

            DialogTimeCode = node.playerDialogData.timeCode;
            
            SendDialogEvent(CurrentDialogNode.playerDialogData.text);
            Fsm.OnClick += MouseClicked;
        }

        public override void Update()
        {
            if (!IsCompleted) 
                base.Update(); //Проверка TimesCodes
        }

        private void MouseClicked()
        {
            if (!IsCompleted)
                Complete();
        }

        protected override void Complete()
        {
            ChangeDialogState<NPCDialogState>();
        }

        public override void Exit()
        {
            base.Exit();
            Fsm.OnClick -= MouseClicked;
        }
    }
}