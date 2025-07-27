using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using StateMachin.States;

namespace Actors.NPC.DialogSystem
{
    public class DialogFSM
    {
        private DialogState _currentState;
        private Dictionary<Type, DialogState> _states = new Dictionary<Type, DialogState>();
        
        public Action<string> OnSendActorText;
        public Action<List<DialogNode>> OnSendDialogNodes;
        public Action<DialogNode> OnStartDialog;
        public Action OnClick;


        public void EnterToIdleState()
        {
            var idleState = _states[typeof(IdleDialogState)];

            if (idleState is IdleDialogState idleDialogState)
            {
                idleDialogState.EnterToIdle();
                _currentState = idleDialogState;
            }
        }

        public void AddNewState(DialogState dialogState)
        {
            _states.TryAdd(dialogState.GetType(), dialogState);
        }
        
        public void ChangeState<T>(DialogNode dialogNode = null) where T : DialogState
        {
            var type = typeof(T);

            if (_currentState?.GetType() == type) return;

            if (_states.TryGetValue(type, out var state))
            {
                _currentState?.Exit();
                _currentState = state;
                state.Enter(dialogNode);
            }
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}