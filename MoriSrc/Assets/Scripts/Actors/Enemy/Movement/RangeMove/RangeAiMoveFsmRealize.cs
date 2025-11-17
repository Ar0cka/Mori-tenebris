using System;
using System.Collections.Generic;
using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.RangeMove.States;
using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.RangeMove.States;
using Actors.Enemy.Movement.Service;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Movement.RangeMove
{
    public class RangeAiMoveFsmRealize : EnemyMoveFsmRealize
    {
        [SerializeField] private RangeMoveData rangeData;
        protected override MoveData MoveData => rangeData;
        public RadiusService<RadiusSettings> RadiusService { get; private set; }

        private Dictionary<AiRadiusEnum, float> _radiusDictionary;

        public override void Initialize()
        {
            _radiusDictionary = rangeData.RadiusSettings.RadiusDictionary;
            
            RadiusService = new RadiusService<RadiusSettings>(rangeData.RadiusSettings);
            
            base.Initialize();
            
            StatesInit();
            
            FsmUnityBase.ChangeState<RangeIdleState>();
        }

        protected override void StatesInit()
        {
            var baseContext = new BaseMovementContext<RangeMoveData, EnemyMoveFsm>(rangeData, FsmUnityBase, rb2D,
                animator, spriteRenderer, DetectedPlayerService);
            
            FsmUnityBase.AddState(new RangeReturnToStart(new BaseMovementContext<MoveData, EnemyMoveFsm>(rangeData, FsmUnityBase, rb2D,
                animator, spriteRenderer, DetectedPlayerService), new Vector2(rb2D.position.x, rb2D.position.y), rangeData.RadiusSettings));
            FsmUnityBase.AddState(new RangeIdleState(baseContext, RadiusService));
            FsmUnityBase.AddState(new SmallRadiusState(baseContext, RadiusService));
            FsmUnityBase.AddState(new MediumRadiusState(baseContext, RadiusService));
            FsmUnityBase.AddState(new LargeRadiusState(baseContext, RadiusService));
        }

        protected override void DrawGizmos()
        {
            if (rangeData == null)
                return;

            if (_radiusDictionary == null)
                _radiusDictionary = rangeData.RadiusSettings.RadiusDictionary;

            if (_radiusDictionary != null)
            {
                if (_radiusDictionary.Count == 0)
                    return;
                try
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position, _radiusDictionary[AiRadiusEnum.Small]);
                    
                    Vector2 lookDirection =
                        spriteRenderer != null && spriteRenderer.flipX ? Vector2.right : Vector2.left;

                    float halfAngle = rangeData.IdleDetectionSettings.fieldOfViewAngle / 2;

                    Vector3 leftBoundary = Quaternion.Euler(0, 0, halfAngle) * lookDirection;
                    Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * lookDirection;

                    Gizmos.color = Color.aquamarine;
                    Gizmos.DrawRay(transform.position, leftBoundary * _radiusDictionary[AiRadiusEnum.Medium]);
                    Gizmos.DrawRay(transform.position, rightBoundary * _radiusDictionary[AiRadiusEnum.Medium]);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, _radiusDictionary[AiRadiusEnum.Medium]);

                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position, _radiusDictionary[AiRadiusEnum.Large]);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error drawing range: {e.Message}");
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawGizmos();
        }
#endif
    }
}