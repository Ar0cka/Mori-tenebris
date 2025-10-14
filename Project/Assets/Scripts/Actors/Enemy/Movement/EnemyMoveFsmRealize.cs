using System.Collections.Generic;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    public class EnemyMoveFsmRealize : FsmRealizeBase<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        [Header("Move data")]
        [SerializeField] private MoveData moveData;
        [SerializeField] private List<Vector2> waypoints;
        
        [Header("Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D rb2D;

        public bool OnSeePlayer { get; private set; }
        
        public override void Initialize()
        {
            FsmUnityBase = new EnemyMoveFsm();

            Vector2 currentPosition = transform.position;
            
            moveData.PatrolSettings.SetPatrolPoints(waypoints);
            
            FsmUnityBase.AddState(new IdleMoveState(FsmUnityBase, this));
            FsmUnityBase.AddState(new PursuitPlayer(FsmUnityBase, rb2D, this, 
                moveData.MoveSettings, moveData.AggressiveSettings));
            FsmUnityBase.AddState(new ReturnToStartPosition(FsmUnityBase, this, rb2D,
                new Vector2(currentPosition.x, currentPosition.y), moveData.MoveSettings));
            FsmUnityBase.AddState(new PatrolMoveState(FsmUnityBase, this, rb2D, moveData.PatrolSettings));

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

        public void ChangeViewState(bool onSeePlayer)
        {
            OnSeePlayer = onSeePlayer;
        }

        //Service for get position player and check his collider in zone
        #region Physics Service 

        public bool DetectTarget()
        {
            var detectionData = moveData.IdleDetectionSettings;
            
            var hits =
                Physics2D.OverlapCircle(transform.position, detectionData.idleDetectionRadius, LayerMask.GetMask("Player"));

            if (hits is null)
            {
                Debug.Log("Not find player");
                return false;
            }
            
            Vector2 lookDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
            Vector2 targetDirection = (hits.transform.position - transform.position).normalized;
            
            float angle = Vector2.Angle(lookDirection, targetDirection);

            if (angle > detectionData.fieldOfViewAngle / 2)
            {
                Debug.Log("Not find player");
                return false;
            }
            
            Debug.Log(angle + " player detection");
            ChangeViewState(true);
            return true;
        }

        public Vector2 GetTargetPosition()
        {
            var agrSettings = moveData.AggressiveSettings;
            
            Collider2D hit = Physics2D.OverlapCircle(transform.position, agrSettings.detectionRadius, LayerMask.GetMask("Player"));
            
            if (hit is null)
                return Vector2.zero;
            
            return hit.transform.position;
        }

        #endregion 
        

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