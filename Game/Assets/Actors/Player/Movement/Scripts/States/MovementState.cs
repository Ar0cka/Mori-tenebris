using Actors.Player.AttackSystem;
using Actors.Player.Movement.Scripts;
using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;


public class MovementState : State
{
    protected Rigidbody2D rb2D;
    protected SpriteRenderer _spriteRenderer;
    protected MovementColliderOffset MovementColliderOffset;
    protected CapsuleCollider2D _capsuleCollider;
    protected Animator _animator;

    public MovementState(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr, Rigidbody2D _rb2D, 
        SpriteRenderer spriteRenderer, MovementColliderOffset movementColliderOffset, CapsuleCollider2D capsuleCollider, Animator animator) :
        base(fsm, stateMachineRealize, playerScr)
    {
        _playerScrObj = playerScr;
        rb2D = _rb2D;
        _spriteRenderer = spriteRenderer;
        MovementColliderOffset = movementColliderOffset;
        _capsuleCollider = capsuleCollider;
        _animator = animator;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FStateMachine.ChangeState<SprintRun>();
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        Move(_playerScrObj.StaticPlayerStats.WalkSpeed);
        ChangeSpriteSide(GetInput());
    }

    public override void SetAnimation()
    {
        _animator.SetBool("Walk", animationState);
    }

    public void UpdateMovementOffset(MovementColliderOffset movementColliderOffset)
    {
        MovementColliderOffset = movementColliderOffset;
    }
    
    protected void Move(float speed)
    {
        rb2D.MovePosition(rb2D.position + GetInput().normalized * speed * Time.deltaTime);
    }
    protected void ChangeSpriteSide(Vector2 inputVector)
    {
        if (!PlayerStates.IsBusy && inputVector.x != 0)
        {
            _spriteRenderer.flipX = inputVector.x < 0;
            _capsuleCollider.offset = inputVector.x < 0 ? MovementColliderOffset.MoveLeftOffset : MovementColliderOffset.MoveRightOffset;
        }
    }
}