using UnityEngine;

namespace FiniteStateMachine
{
    public abstract class FsmRealizeBase<TFsm, TState> : MonoBehaviour 
        where TFsm : FsmUnityBase<TFsm, TState> 
        where TState : FsmUnityState<TFsm, TState>
    {
        protected TFsm FsmUnityBase;
        
        public abstract void Initialize();

        protected virtual void Update()
        {
            FsmUnityBase?.Update();
        }

        protected virtual void FixedUpdate()
        {
            FsmUnityBase?.PhysicsUpdate();
        }
    }
}