using System;
using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        protected readonly MileEnemyMoveRealize Realize;
        protected readonly MoveData MoveData;
        protected readonly SpriteRenderer SpriteRenderer;
        protected readonly Rigidbody2D Rb2D;
        protected readonly Animator Animator;
        protected readonly DetectedPlayerService DetectedPlayerService;
        
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm, DataContext<MileEnemyMoveRealize, MoveData> dataContext, 
            BaseMovementContext baseMovementContext) : base(fsm)
        {
            Realize = dataContext.Realize;
            MoveData = dataContext.Config;
            SpriteRenderer = baseMovementContext.SpriteRenderer;
            Rb2D = baseMovementContext.Rigidbody2D;
            Animator = baseMovementContext.Animator;
            DetectedPlayerService = baseMovementContext.DetectedPlayerService;
        }

        protected void ChooseIdleState(MoveSettings moveSettings)
        {
            if (moveSettings.hasPatrol)
                StateMachine.ChangeState<PatrolMoveState>();
            else
                StateMachine.ChangeState<ReturnToStartPosition>();
            
            Realize.ChangeViewState(false);
        }

        protected virtual Vector2 DetectedPlayer()
        {
            Vector2 targetPos = DetectedPlayerService.DetectedTarget(MoveData.AggressiveSettings.detectionRadius, MoveData.TargetMask, Rb2D.position);
            
            return targetPos;
        }

        protected virtual bool IdleDetected()
        {
            var idleContext = MoveData.IdleDetectionSettings;
            
            return DetectedPlayerService.IdleDetection(idleContext.idleDetectionRadius, idleContext.fieldOfViewAngle, 
                MoveData.TargetMask, SpriteRenderer, Rb2D.position);
        }
    }
    
    public class BaseMovementContext
    {
        public SpriteRenderer SpriteRenderer;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
        public DetectedPlayerService DetectedPlayerService;

        public BaseMovementContext(MileEnemyMoveRealize fsmRealize,
            SpriteRenderer fsmSpriteRenderer, Rigidbody2D fsmRigidbody2D,
            Animator fsmAnimator, MoveData fsmMoveData, DetectedPlayerService fsmDetectedPlayerService)
        {
            SpriteRenderer = fsmSpriteRenderer;
            Rigidbody2D = fsmRigidbody2D;
            Animator = fsmAnimator;
            DetectedPlayerService = fsmDetectedPlayerService;
        }
    }

    public class DataContext<TRealize, TConfig>
    {
        public TRealize Realize;
        public TConfig Config;

        public DataContext(TRealize realize, TConfig config)
        {
            Realize = realize;
            Config = config;
        }
    }
}