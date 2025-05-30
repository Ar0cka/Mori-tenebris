public abstract class State
{
    protected FStateMachine FStateMachine;
    protected StateMachineRealize _stateMachineRealize;
    protected bool animationState;

    protected State(FStateMachine fsm, StateMachineRealize stateMachineRealize)
    {
        FStateMachine = fsm;
        _stateMachineRealize = stateMachineRealize;
        animationState = false;
    }
    
    public virtual void Enter()
    {
        animationState = true;
        SetAnimation();
    }

    public virtual void Exit()
    {
        animationState = false;
        SetAnimation();
    }

    public virtual void UpdateLogic()
    {
    }

    public virtual void PhysicUpdate()
    {
    }

    public virtual void SetAnimation()
    {
    }
}