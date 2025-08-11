using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.FSM.DialogStates;
using Actors.NPC.DialogSystem.TestUI;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public abstract class DialogState : IDialogState
    {
        protected DialogNode CurrentDialogNode;
        protected bool IsEnterToState = false;
        protected bool IsCompleted = false;

        protected float DialogTimeCode;

        protected DialogFSM Fsm;
        public DialogState(DialogFSM stateDialogFsm)
        {
            Fsm = stateDialogFsm;
        }
        
        public virtual void Enter(DialogNode node)
        {
            if (node == null)
            {
                Debug.Log("Enter null node");
                Fsm.EnterToIdleState();
            }
            
            CurrentDialogNode = node;
            IsEnterToState = true;
        }

        public virtual void Update()
        {
            if (!IsEnterToState || IsCompleted) return;
            
            DialogTimeCode -= Time.deltaTime;
            if (DialogTimeCode <= 0)
                Complete();
        }

        public virtual void Exit()
        {
            IsEnterToState = false;
            CurrentDialogNode = null;
            IsCompleted = false;
        }
        
        protected void SendDialogEvent(string text)
        {
            Fsm.OnSendActorText?.Invoke(text);
        }

        protected void SendDialogsNodes()
        {
            if (CurrentDialogNode != null) 
                Fsm.OnSendDialogNodes?.Invoke(CurrentDialogNode.GetNextNodes());
        }

        protected void ChangeDialogState<T>() where T : DialogState
        {
            Fsm.ChangeState<T>(CurrentDialogNode);
        }
    
        protected virtual void Complete()
        {
            IsCompleted = true;
        }

        protected bool CheckAndSwitchOnPanelState()
        {
            if (CurrentDialogNode != null && CurrentDialogNode.Condition.ActionType == DialogActionType.OpenPanel)
            {
                ChangeDialogState<OpenNpcPanel>();
                return true;
            }

            return false;
        }
    }

    public interface IDialogState
    {
        void Enter(DialogNode node);
        void Update();
        void Exit();
    }
}