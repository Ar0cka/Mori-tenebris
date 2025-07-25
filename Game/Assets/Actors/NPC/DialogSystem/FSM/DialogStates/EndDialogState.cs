using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;

namespace Actors.NPC.DialogSystem
{
    public class EndDialogState : DialogState
    {
        public EndDialogState(DialogFSM fsm, Action<List<DialogNode>> sendDialogsNodes) : base(fsm)
        {
            Fsm = fsm;
            SendNextDialogNodes = sendDialogsNodes;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            SendDialogEvent("Диалог завершён.");
            Exit();
        }

        public override void FixedUpdate()
        {
            // Конечное состояния, отключаем базовый FixedUpdate
        }

        public override void Exit()
        {
            SendDialogsNodes();
            base.Exit();
        }
    }
}