using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        protected readonly EnemyMoveFsmRealize FsmRealize;
        protected readonly MoveData MoveData;
        protected readonly SpriteRenderer SpriteRenderer;
        protected readonly Rigidbody2D Rb2D;
        protected readonly Animator Animator;
        protected readonly DetectedPlayerService DetectedPlayerService;
        
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm, BaseMovementContext baseMovementContext) : base(fsm)
        {
            FsmRealize = baseMovementContext.FsmRealize;
            MoveData = baseMovementContext.MoveData;
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
            
            FsmRealize.ChangeViewState(false);
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
        public readonly EnemyMoveFsmRealize FsmRealize;
        public readonly MoveData MoveData;
        public readonly SpriteRenderer SpriteRenderer;
        public readonly Rigidbody2D Rigidbody2D;
        public readonly Animator Animator;
        public readonly DetectedPlayerService DetectedPlayerService;
    
        public BaseMovementContext(EnemyMoveFsmRealize enemyMoveFsmRealize,
            SpriteRenderer spriteRenderer, Rigidbody2D rigidbody2D, Animator animator,
            MoveData moveData, DetectedPlayerService detectedPlayerService)
        {
            FsmRealize = enemyMoveFsmRealize;
            MoveData = moveData;
            SpriteRenderer = spriteRenderer;
            Rigidbody2D = rigidbody2D;
            Animator = animator;
            DetectedPlayerService =  detectedPlayerService;
        }
    }
}