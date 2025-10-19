using System;
using Actors.Enemy.Movement;
using Actors.Enemy.Movement.Service;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class BaseMovementState : MoveEnemyFsmUnityState
    {
        protected Vector2 CurrentVelocity = Vector2.zero;

        private string _currentAnimationName;

        public override void Enter()
        {
            SetAnimation(MoveType.Move);
        }

        public BaseMovementState(EnemyMoveFsm enemyMoveFsm, BaseMovementContext baseMovementContext) : base(
            enemyMoveFsm, baseMovementContext)
        {
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
            var dictionary = MoveData.MoveSettings.movementAnimationList.Dictionary;

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