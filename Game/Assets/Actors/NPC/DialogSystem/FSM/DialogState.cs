using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public abstract class DialogState
    {
        protected DialogNode CurrentDialogNode;
        protected bool IsEnterToState = false;
        protected bool IsCompleted = false;

        protected Action<string> SendDialogData;
        protected Action<List<DialogNode>> SendNextDialogNodes;

        protected float DialogTimeCode;

        protected DialogFSM Fsm;
        public DialogState(DialogFSM stateDialogFsm)
        {
            Fsm = stateDialogFsm;
        }
        
        public virtual void Enter(DialogNode node)
        {
            CurrentDialogNode = node;
            IsEnterToState = true;
        }

        public virtual void FixedUpdate()
        {
            if (!IsEnterToState || IsCompleted) return;
            
            DialogTimeCode -= Time.fixedDeltaTime;
            if (DialogTimeCode <= 0)
                Complete();
        }

        public virtual void Exit()
        {
            IsEnterToState = false;
            CurrentDialogNode = null;
        }

        protected void SendDialogEvent(string text)
        {
            SendDialogData?.Invoke(text);
        }

        protected void SendDialogsNodes()
        {
            SendNextDialogNodes?.Invoke(CurrentDialogNode.GetNextNodes());
        }

        protected void ChangeDialogState<T>() where T : DialogState
        {
            Fsm.ChangeState<T>(CurrentDialogNode);
        }
    
        protected virtual void Complete()
        {
            IsCompleted = true;
        }
    }
}