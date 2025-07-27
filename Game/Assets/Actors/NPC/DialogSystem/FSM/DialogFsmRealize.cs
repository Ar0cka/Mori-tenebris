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
        private bool _isInitialize = false;

        public void Initialize()
        {
            fsm = new DialogFSM();
            
            fsm.AddNewState(new IdleDialogState(fsm));
            fsm.AddNewState(new PlayerDialogState(fsm));
            fsm.AddNewState(new NPCDialogState(fsm));
            fsm.AddNewState(new EndDialogState(fsm));
            
            fsm.EnterToIdleState();
            
            _isInitialize = true;
        }

        private void FixedUpdate()
        {
            if (!_isInitialize) return;
            
            fsm.Update();
        }

        public DialogFSM GetDialogFsm() => fsm;
    }
}