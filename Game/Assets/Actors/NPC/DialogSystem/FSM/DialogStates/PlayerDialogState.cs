using System;
using Actors.NPC.DialogSystem.DataScripts;
using UnityEngine;

namespace Actors.NPC.DialogSystem.DialogStates
{
    public class PlayerDialogState : DialogState
    {
        protected Action OnClick;
        private bool _isDone;
        
        public PlayerDialogState(DialogFSM fsm, Action<string> sendTextDatAction, Action onClick) : base(fsm)
        {
            Fsm = fsm;
            SendDialogData = sendTextDatAction;
            OnClick = onClick;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);

            DialogTimeCode = node.playerDialogData.timeCode;
            
            SendDialogEvent(CurrentDialogNode.playerDialogData.text);
            OnClick += MouseClicked;
        }

        public override void FixedUpdate()
        {
            if (!_isDone) 
                base.FixedUpdate(); //Проверка TimesCodes
        }

        private void MouseClicked()
        {
            if (!_isDone)
                Complete();
        }

        protected override void Complete()
        {
            _isDone = true;
            ChangeDialogState<NPCDialogState>();
        }

        public override void Exit()
        {
            base.Exit();
            OnClick -= MouseClicked;
        }
    }
}