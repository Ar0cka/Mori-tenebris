using System;
using Actors.Enemy.Movement;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class BaseMovementState : MoveEnemyFsmUnityState
    {
        protected EnemyMoveFsmRealize FsmRealize;
        protected MoveSettings MoveSettings;
        protected SpriteRenderer SpriteRenderer;
        protected Rigidbody2D Rb2D;
        protected Animator Animator;

        protected Vector2 CurrentVelocity = Vector2.zero;

        private string _currentAnimationName;

        public override void Enter()
        {
            SetAnimation(MoveType.Move);
        }

        public BaseMovementState(EnemyMoveFsm enemyMoveFsm, BaseMovementContext baseMovementContext) : base(
            enemyMoveFsm)
        {
            FsmRealize = baseMovementContext.FsmRealize;
            MoveSettings = baseMovementContext.MoveSettings;
            SpriteRenderer = baseMovementContext.SpriteRenderer;
            Rb2D = baseMovementContext.Rigidbody2D;
            Animator = baseMovementContext.Animator;
        }
        
        public override void PhysicsUpdate()
        {
            CheckObstacle();
        }
        protected void CheckObstacle()
        {
            if (CurrentVelocity == Vector2.zero)
                return;
            
            var ray = Physics2D.Raycast(Rb2D.position, CurrentVelocity.normalized, 5f, LayerMask.GetMask("Obstacle"));

            if (ray.collider != null)
            {
                StateMachine.ChangeState<PathfinderMove>();
            }
        }
        protected virtual void SetAnimation(MoveType moveType)
        {
            var dictionary = MoveSettings.movementAnimationList.Dictionary;

            if (dictionary == null)
               throw new Exception("No movement animation list found");
            
            if (dictionary.TryGetValue(moveType, out var animation))
            {   
                if (_currentAnimationName == animation)
                    return;
                
                if (string.IsNullOrEmpty(animation))
                    throw new Exception("Not find animation with type: " + moveType);
                
                if (!string.IsNullOrEmpty(_currentAnimationName)) 
                    Animator.SetBool(_currentAnimationName, false);
                
                Animator.SetBool(animation, true);
                _currentAnimationName = animation;
            }
        }
        protected virtual void SetSpriteSide()
        {
            if (CurrentVelocity == Vector2.zero)
            {
                SpriteRenderer.flipX = false;
                return;
            }
            
            SpriteRenderer.flipX = CurrentVelocity.x > 0;
        }
        protected bool CheckDistance(Vector2 targetPosition, Vector2 currentPosition, float distance)
        {
            return Vector2.Distance(targetPosition, currentPosition) <= distance;
        }
        protected virtual void BaseMove(Vector2 targetPos, float speed)
        {
            Vector2 moveDirection = (targetPos - Rb2D.position).normalized;
            CurrentVelocity = moveDirection * speed;
            
            Rb2D.MovePosition(Rb2D.position + CurrentVelocity * Time.fixedDeltaTime);
            
            SetSpriteSide();
        }
        public override void Exit()
        {
            if (_currentAnimationName != null)
                Animator.SetBool(_currentAnimationName, false);
        }
    }
}

public class BaseMovementContext
{
    public readonly EnemyMoveFsmRealize FsmRealize;
    public readonly MoveSettings MoveSettings;
    public readonly SpriteRenderer SpriteRenderer;
    public readonly Rigidbody2D Rigidbody2D;
    public readonly Animator Animator;
    
    public BaseMovementContext(EnemyMoveFsmRealize enemyMoveFsmRealize,
        SpriteRenderer spriteRenderer, Rigidbody2D rigidbody2D, Animator animator,
        MoveSettings moveSettings)
    {
        FsmRealize = enemyMoveFsmRealize;
        MoveSettings = moveSettings;
        SpriteRenderer = spriteRenderer;
        Rigidbody2D = rigidbody2D;
        Animator = animator;
    }
}