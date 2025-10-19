using System.Collections.Generic;
using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement
{
    public class EnemyMoveFsmRealize : FsmRealizeBase<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        [Header("Move data")]
        [SerializeField] protected MoveData moveData;
        [SerializeField] protected List<Vector2> waypoints;
        
        [Header("Components")]
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected Animator animator;
        
        [Inject] protected DetectedPlayerService DetectedPlayerService;
        
        public bool OnSeePlayer { get; private set; }
        
        public override void Initialize()
        {
            FsmUnityBase = new EnemyMoveFsm();
            
            moveData.PatrolSettings.SetPatrolPoints(waypoints);
            moveData.MoveSettings.movementAnimationList.ToDictionary();
            
            StatesInit();
            
            switch (moveData.MoveSettings.hasPatrol)
            {
                case true:
                    FsmUnityBase.ChangeState<PatrolMoveState>();
                    break;
                case false:
                    FsmUnityBase.ChangeState<IdleMoveState>();
                    break;
            } //On starting state
        }

        protected virtual void StatesInit()
        {
            Vector2 currentPosition = transform.position;
            
            BaseMovementContext baseMovementContext =
                new BaseMovementContext(this, spriteRenderer, rb2D, animator, moveData, DetectedPlayerService);
            
            FsmUnityBase.AddState(new IdleMoveState(FsmUnityBase, baseMovementContext));
            FsmUnityBase.AddState(new PursuitPlayer(FsmUnityBase, baseMovementContext));
            FsmUnityBase.AddState(new ReturnToStartPosition(FsmUnityBase, baseMovementContext, 
                new Vector2(currentPosition.x, currentPosition.y)));
            FsmUnityBase.AddState(new PatrolMoveState(FsmUnityBase, baseMovementContext));
            FsmUnityBase.AddState(new LingerState(FsmUnityBase, baseMovementContext));
        }
        
        public void ChangeViewState(bool onSeePlayer)
        {
            OnSeePlayer = onSeePlayer;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (moveData == null || moveData.IdleDetectionSettings == null)
                return;

            var detectionData = moveData.IdleDetectionSettings;

            Gizmos.color = Color.darkRed;
            Gizmos.DrawWireSphere(transform.position, moveData.AggressiveSettings.detectionRadius);
            
            // Цвет радиуса
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionData.idleDetectionRadius);

            // Получаем направление взгляда (как в DetectTarget)
            Vector2 lookDirection = spriteRenderer != null && spriteRenderer.flipX ? Vector2.right : Vector2.left;

            // Левая и правая границы угла обзора
            float halfAngle = detectionData.fieldOfViewAngle / 2f;

            Vector3 leftBoundary = Quaternion.Euler(0, 0, halfAngle) * lookDirection;
            Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * lookDirection;

            // Цвет лучей обзора
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, leftBoundary * detectionData.idleDetectionRadius);
            Gizmos.DrawRay(transform.position, rightBoundary * detectionData.idleDetectionRadius);

            if (waypoints is null || waypoints.Count == 0)
                return;
            
            foreach (var waypoint in waypoints)
            {
                Gizmos.color = Color.deepPink;
                Gizmos.DrawSphere(waypoint, 0.5f);
            }
        } 
#endif //Gizmos for drawing sphere and angle 
    }
}