using Actors.Player.AbstractFSM;
using Actors.Player.AttackSystemFSM;

public abstract class State
{
    protected FStateMachine FStateMachine;
    protected AbstractStateMachineRealize _stateMachineRealize;
    protected bool animationState;

    protected State(FStateMachine fsm, AbstractStateMachineRealize stateMachineRealize)
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