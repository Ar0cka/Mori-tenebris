namespace FiniteStateMachine
{
    public abstract class StateAbstract<TFsm, TState> 
        where TState : StateAbstract<TFsm, TState>
        where TFsm : FsmAbstract<TFsm, TState>
    {
        protected TFsm StateMachine;
        
        public StateAbstract(TFsm fsm)
        {
            StateMachine = fsm;
        }
        
        public virtual void Enter() { }
        
        public virtual void Update() { }
        
        public virtual void PhysicsUpdate() { }
        
        public virtual void Exit() { }
    }
}