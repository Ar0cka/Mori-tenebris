namespace FiniteStateMachine
{
    public abstract class FsmUnityState<TFsm, TState> 
        where TState : FsmUnityState<TFsm, TState>
        where TFsm : FsmUnityBase<TFsm, TState>
    {
        protected TFsm StateMachine;
        
        public FsmUnityState(TFsm fsm)
        {
            StateMachine = fsm;
        }
        
        public virtual void Enter() { }
        
        public virtual void Update() { }
        
        public virtual void PhysicsUpdate() { }
        
        public virtual void Exit() { }
    }
}