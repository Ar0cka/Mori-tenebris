using System;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.DialogSystem.FSM.DialogStates;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class DialogFsmRealize : MonoBehaviour
    {
        [SerializeField] private NpcController npcController;
        
        private DialogFSM fsm;
        private bool _isInitialize = false;

        public void Initialize()
        {
            fsm = new DialogFSM();
            
            fsm.AddState(new IdleDialogState(fsm));
            fsm.AddState(new PlayerDialogState(fsm));
            fsm.AddState(new NPCDialogState(fsm, npcController));
            fsm.AddState(new OpenNpcPanel(fsm));
            fsm.AddState(new IdlePanelState(fsm));
            fsm.AddState(new EndDialogState(fsm));
            
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