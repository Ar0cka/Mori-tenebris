using System;
using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.Service;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class BaseMovementState<TFsm, TConfig> : FsmState
        where TConfig : MoveData 
        where TFsm : FsmUnityBase
    {
        protected BaseMovementContext<TConfig, TFsm> Ctx;
        
        protected Vector2 CurrentVelocity = Vector2.zero;

        private string _currentAnimationName;
        
        public override void Enter()
        {
            SetAnimation(MoveType.Move);
        }

        public BaseMovementState(BaseMovementContext<TConfig, TFsm> moveContext)
        {
            Ctx = moveContext;
        }
        
        public override void PhysicsUpdate()
        {
            CheckObstacle();
        }
        protected void CheckObstacle()
        {
            if (CurrentVelocity == Vector2.zero)
                return;
            
            var ray = Physics2D.Raycast(Ctx.Rb2D.position, CurrentVelocity.normalized, 5f, LayerMask.GetMask("Obstacle"));

            if (ray.collider != null)
            {
                Ctx.Fsm.ChangeState<PathfinderMove>();
            }
        }
        protected virtual void SetAnimation(MoveType moveType)
        {
            var dictionary = Ctx.Config.MoveSettings.movementAnimationList.Dictionary;

            if (dictionary == null)
               throw new Exception("No movement animation list found");
            
            if (dictionary.TryGetValue(moveType, out var animation))
            {   
                if (_currentAnimationName == animation)
                    return;
                
                if (string.IsNullOrEmpty(animation))
                    throw new Exception("Not find animation with type: " + moveType);
                
                if (!string.IsNullOrEmpty(_currentAnimationName)) 
                    Ctx.Animator.SetBool(_currentAnimationName, false);
                
                Ctx.Animator.SetBool(animation, true);
                _currentAnimationName = animation;
            }
        }
        protected virtual void SetSpriteSide()
        {
            if (CurrentVelocity == Vector2.zero)
            {
                Ctx.SpriteRenderer.flipX = false;
                return;
            }
            
            Ctx.SpriteRenderer.flipX = CurrentVelocity.x > 0;
        }
        
        protected virtual void BaseMove(Vector2 targetPos, float speed)
        {
            Vector2 moveDirection = (targetPos - Ctx.Rb2D.position).normalized;
            CurrentVelocity = moveDirection * speed;
            
            Ctx. Rb2D.MovePosition(Ctx.Rb2D.position + CurrentVelocity * Time.fixedDeltaTime);
            
            SetSpriteSide();
        }
        
        protected void ChooseIdleState<TIdle, TPatrol>(MoveSettings moveSettings) 
            where TIdle : FsmState
            where TPatrol : FsmState
        {
            if (moveSettings.hasPatrol)
                Ctx.Fsm.ChangeState<TPatrol>();
            else
                Ctx.Fsm.ChangeState<TIdle>();
        }

        protected virtual Vector2 DetectedPlayer(float radius, LayerMask targetMask)
        {
            Vector2 targetPos = Ctx.DetectedPlayerService.DetectedTarget(radius, targetMask, Ctx.Rb2D.position);
            
            return targetPos;
        }

        protected virtual bool IdleDetected(MoveData moveData)
        {
            var idleContext = moveData.IdleDetectionSettings;
            
            return Ctx.DetectedPlayerService.IdleDetection(idleContext.idleDetectionRadius, idleContext.fieldOfViewAngle, 
                moveData.TargetMask, Ctx.SpriteRenderer, Ctx.Rb2D.position);
        }
        protected bool CheckDistance(Vector2 targetPosition, Vector2 currentPosition, float distance)
        {
            return Vector2.Distance(targetPosition, currentPosition) <= distance;
        }
        protected void ChangeState<TType>() where TType : FsmState
        {
            Ctx.Fsm.ChangeState<TType>();
        }
        
        public override void Exit()
        {
            if (_currentAnimationName != null)
                Ctx.Animator.SetBool(_currentAnimationName, false);
        }
    }
}