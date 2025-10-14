using System;
using Actors.Enemy.Movement.MovementFsm.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.MovementFsm
{
    public class EnemyMoveFsmRealize : FsmRealizeBase<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        [SerializeField] private MoveData moveData;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public bool OnSeePlayer { get; private set; }
        
        public override void Initialize()
        {
            FsmUnityBase = new EnemyMoveFsm();
            
            FsmUnityBase.AddState(new IdleMoveState(FsmUnityBase, this));
            
            FsmUnityBase.ChangeState<IdleMoveState>();
        }

        public void ChangeViewState(bool onSeePlayer)
        {
            OnSeePlayer = onSeePlayer;
        }
        
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
            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (moveData == null || moveData.IdleDetectionSettings == null)
                return;

            var detectionData = moveData.IdleDetectionSettings;

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
        }
#endif
       
        
    }
}