using StateMachin.States;
using UnityEngine;


public class MovementState : State
{
    protected PlayerScrObj _playerScr;
    protected Rigidbody2D rb2D;

    public MovementState(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr, Rigidbody2D _rb2D) :
        base(fsm, stateMachineRealize)
    {
        _playerScr = playerScr;
        rb2D = _rb2D;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector2 input = InputVector();

        if (input.sqrMagnitude == 0)
        {
            FStateMachine.ChangeState<IdleState>();
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        Move(_playerScr.StaticPlayerStats.walkSpeed);
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        _stateMachineRealize.SetWalkAnimation(animationState);
    }

    protected void Move(float speed)
    {
        rb2D.MovePosition(rb2D.position + InputVector().normalized * speed * Time.deltaTime);
    }

    protected Vector2 InputVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}