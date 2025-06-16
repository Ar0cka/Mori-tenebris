using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;

public abstract class State
{
    protected FStateMachine FStateMachine;
    protected StateMachineRealize _stateMachineRealize;
    protected PlayerScrObj _playerScrObj;
    protected bool animationState;

    protected State(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScrObj)
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
        Vector2 input = GetInput();
        
        if (input.sqrMagnitude == 0)
        {
            FStateMachine.ChangeState<IdleState>();
        }

        Roll();
    }

    protected void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _stateMachineRealize.RollCooldown <= 0)
        {
            FStateMachine.ChangeState<Roll>();
        }
    }
    
    public virtual void PhysicUpdate()
    {
    }

    public virtual void SetAnimation()
    {
    }

    protected Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}