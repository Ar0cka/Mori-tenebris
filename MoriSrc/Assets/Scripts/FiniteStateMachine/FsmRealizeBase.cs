using UnityEngine;

namespace FiniteStateMachine
{
    public abstract class FsmRealizeBase<TFsm> : MonoBehaviour where TFsm : FsmUnityBase
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