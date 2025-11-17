using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Base.Service;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement.Base
{
    public abstract class EnemyMoveFsmRealize : FsmRealizeBase<EnemyMoveFsm> 
    {
        [Header("Configs")]
        protected abstract MoveData MoveData { get; }
        
        [Header("Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        [Inject] protected DetectedPlayerService DetectedPlayerService;
        
        public bool OnSeePlayer { get; private set; }
        protected bool Initialized = false;

        public override void Initialize()
        {
            FsmUnityBase = new EnemyMoveFsm();
            MoveData.MoveSettings.movementAnimationList.ToDictionary();
        }

        protected virtual void StatesInit()
        {
            Vector2 currentPosition = transform.position;
            
            var baseMovementContext = 
                new BaseMovementContext<MoveData, EnemyMoveFsm>(MoveData, FsmUnityBase, rb2D, animator, spriteRenderer, DetectedPlayerService);
            
            FsmUnityBase.AddState(new MileIdle(baseMovementContext));
            FsmUnityBase.AddState(new PursuitPlayer(baseMovementContext));
            FsmUnityBase.AddState(new ReturnToStartPosition(baseMovementContext, 
                new Vector2(currentPosition.x, currentPosition.y)));
            FsmUnityBase.AddState(new PatrolMoveState(baseMovementContext));
            FsmUnityBase.AddState(new LingerState(baseMovementContext));
        }
        public void ChangeViewState(bool onSeePlayer)
        {
            OnSeePlayer = onSeePlayer;
        }
        
#if UNITY_EDITOR
        protected virtual void DrawGizmos()
        {
            if (MoveData == null || MoveData.IdleDetectionSettings == null)
                return;

            var detectionData = MoveData.IdleDetectionSettings;

            Gizmos.color = Color.darkRed;
            Gizmos.DrawWireSphere(transform.position, MoveData.AggressiveSettings.detectionRadius);
            
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

            var waypoints = MoveData.PatrolSettings.patrolPoints;
            
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