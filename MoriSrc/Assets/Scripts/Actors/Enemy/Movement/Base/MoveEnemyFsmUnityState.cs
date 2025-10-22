using System;
using Actors.Enemy.Movement.Base.Service;
using Actors.Enemy.Movement.Base.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        protected readonly SpriteRenderer SpriteRenderer;
        protected readonly Rigidbody2D Rb2D;
        protected readonly Animator Animator;
        protected readonly DetectedPlayerService DetectedPlayerService;
        
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm, BaseMovementContext baseMovementContext) : base(fsm)
        {
            SpriteRenderer = baseMovementContext.SpriteRenderer;
            Rb2D = baseMovementContext.Rigidbody2D;
            Animator = baseMovementContext.Animator;
            DetectedPlayerService = baseMovementContext.DetectedPlayerService;
        }

        protected void ChooseIdleState<TIdle, TPatrol>(MoveSettings moveSettings) 
            where TIdle : MoveEnemyFsmUnityState
            where TPatrol : MoveEnemyFsmUnityState
        {
            if (moveSettings.hasPatrol)
                StateMachine.ChangeState<TPatrol>();
            else
                StateMachine.ChangeState<TIdle>();
        }

        protected virtual Vector2 DetectedPlayer(float radius, LayerMask targetMask)
        {
            Vector2 targetPos = DetectedPlayerService.DetectedTarget(radius, targetMask, Rb2D.position);
            
            return targetPos;
        }

        protected virtual bool IdleDetected(MoveData moveData)
        {
            var idleContext = moveData.IdleDetectionSettings;
            
            return DetectedPlayerService.IdleDetection(idleContext.idleDetectionRadius, idleContext.fieldOfViewAngle, 
                moveData.TargetMask, SpriteRenderer, Rb2D.position);
        }

        protected void ChangeState<TType>() where TType : MoveEnemyFsmUnityState
        {
            StateMachine.ChangeState<TType>();
        }
    }
    
    public class BaseMovementContext
    {
        public SpriteRenderer SpriteRenderer;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
        public DetectedPlayerService DetectedPlayerService;

        public BaseMovementContext(SpriteRenderer fsmSpriteRenderer, Rigidbody2D fsmRigidbody2D,
            Animator fsmAnimator, DetectedPlayerService fsmDetectedPlayerService)
        {
            SpriteRenderer = fsmSpriteRenderer;
            Rigidbody2D = fsmRigidbody2D;
            Animator = fsmAnimator;
            DetectedPlayerService = fsmDetectedPlayerService;
        }
    }

    public class DataContext<TRealize, TConfig>
    {
        public TConfig Config;
        public TRealize Realize;

        public DataContext(TConfig config, TRealize realize)
        {
            Config = config;
            Realize = realize;
        }
    }
}