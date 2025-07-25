using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class DialogFsmRealize : MonoBehaviour
    {
        private DialogFSM fsm;
        private Action<DialogNode> _onStartDialog;
        private bool _isInitialize = false;

        public void Initialize(TestDialogUI ui)
        {
            _onStartDialog = ui.OnStartDialog;
            
            fsm = new DialogFSM();
            
            fsm.AddNewState(new IdleDialogState(fsm));
            fsm.AddNewState(new PlayerDialogState(fsm, ui.OnSendActorText, ui.OnClick));
            fsm.AddNewState(new NPCDialogState(fsm, ui.OnSendActorText, ui.OnClick));
            fsm.AddNewState(new EndDialogState(fsm, ui.OnSendDialogNodes));
            
            fsm.EnterToIdleState(_onStartDialog);
            
            _isInitialize = true;
        }

        private void FixedUpdate()
        {
            if (!_isInitialize) return;
            
            fsm.Update();
        }
    }
}