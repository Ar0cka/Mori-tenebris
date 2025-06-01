using Actors.Player.AttackSystem;
using Actors.Player.Movement.Scripts;
using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;


public class MovementState : State
{
    protected PlayerScrObj _playerScr;
    protected Rigidbody2D rb2D;
    protected SpriteRenderer _spriteRenderer;
    protected MovementOffsetScr _movementOffsetScr;
    protected CapsuleCollider2D _capsuleCollider;
    protected Animator _animator;

    public MovementState(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr, Rigidbody2D _rb2D, 
        SpriteRenderer spriteRenderer, MovementOffsetScr movementOffsetScr, CapsuleCollider2D capsuleCollider, Animator animator) :
        base(fsm, stateMachineRealize)
    {
        _playerScr = playerScr;
        rb2D = _rb2D;
        _spriteRenderer = spriteRenderer;
        _movementOffsetScr = movementOffsetScr;
        _capsuleCollider = capsuleCollider;
        _animator = animator;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector2 input = InputVector();

        if (input.sqrMagnitude == 0)
        {
            FStateMachine.ChangeState<IdleState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FStateMachine.ChangeState<SprintRun>();
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        Move(_playerScr.StaticPlayerStats.WalkSpeed);
        ChangeSpriteSide(InputVector().normalized);
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        _animator.SetBool("Walk", InputVector().sqrMagnitude > 0);
    }

    public void UpdateMovementOffset(MovementOffsetScr movementOffsetScr)
    {
        _movementOffsetScr = movementOffsetScr;
    }
    
    protected void Move(float speed)
    {
        rb2D.MovePosition(rb2D.position + InputVector().normalized * speed * Time.deltaTime);
    }

    protected Vector2 InputVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected void ChangeSpriteSide(Vector2 inputVector)
    {
        if (!GlobalAttackStates.IsBusy)
        {
            _spriteRenderer.flipX = inputVector.x < 0;
            _capsuleCollider.offset = inputVector.x < 0 ? _movementOffsetScr.MoveLeftOffset : _movementOffsetScr.MoveRightOffset;
        }
    }
}