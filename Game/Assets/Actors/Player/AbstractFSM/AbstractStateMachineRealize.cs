using System;
using UnityEngine;

namespace Actors.Player.AbstractFSM
{
    public abstract class AbstractStateMachineRealize : MonoBehaviour
    {
        protected FStateMachine StateMachine;
        
        protected bool IsInitialized = false;
        
        public virtual void Initialize()
        {
            StateMachine = new FStateMachine();
        }

        protected virtual void Update()
        {
            if (IsInitialized)
                StateMachine.Update();
        }

        protected virtual void FixedUpdate()
        {
            if (IsInitialized)
                StateMachine.FixedUpdate();
        }
    }
}