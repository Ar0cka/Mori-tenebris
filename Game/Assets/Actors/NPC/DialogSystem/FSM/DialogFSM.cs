using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.NpcStateSystem;
using StateMachin.States;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class DialogFSM : IDisposable
    {
        private DialogState _currentState;
        private Dictionary<Type, DialogState> _states = new Dictionary<Type, DialogState>();

        #region events

        public Action<string> OnSendActorText;
        public Action<List<DialogNode>> OnSendDialogNodes;
        public Action<DialogNode> OnStartDialog;
        public Action OnExitFromDialog;
        public Action<SpecialPanelType> OnOpenSpecialPanel;
        public Action OnClosePanel;
        public Action OnClick;

        #endregion
        
        private bool isEnterToIdle = false;

        public void Initialize()
        {
            OnExitFromDialog += EnterToIdleState;
        }
        
        public void EnterToIdleState()
        {
            if (isEnterToIdle) return;
            
            isEnterToIdle = true;
            
            ChangeState<IdleDialogState>();
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
            else
            {
                Debug.Log("Dont find this state");
            }
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void ExitFromIdleState()
        {
            isEnterToIdle = false;
        }
        
        public void Dispose()
        {
            OnExitFromDialog -= EnterToIdleState;
        }
    }
}