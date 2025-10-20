using Actors.Enemy.Movement.Enums;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class BackStepState : BaseMovementState
    {
        protected Vector2 _currentTarget;
        protected BackStepSettings _backStepSettings;
        
        public BackStepState(EnemyMoveFsm fsm, BaseMovementContext context) : base(fsm, context)
        {
            _backStepSettings = MoveData.BackStepSettings;
        }

        public override void PhysicsUpdate()
        {
            Backstep();
        }

        protected virtual void Backstep()
        {
            Vector2 objectPosition = DetectedPlayer();
            
            if (Vector2.Distance(Rb2D.position, objectPosition) <= _backStepSettings.maxDistance)
            {
                StateMachine.ChangeState<IdleMoveState>(); //В будущем сменить на Idle атаки либо иные виды, например регенирация
            }
            
            if (_currentTarget == Vector2.zero || Vector2.Distance(Rb2D.position, _currentTarget) <= _backStepSettings.distanceForChangeVector)
            {
                if (objectPosition == Vector2.zero)
                {
                    ChooseIdleState(MoveData.MoveSettings);
                    return;
                }
                
                _currentTarget = Rb2D.position - objectPosition;
                
                Vector2 offset = MoveData.BackStepSettings.backStepOffset;
                
                _currentTarget += new Vector2(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y));
                _currentTarget = _currentTarget.normalized;
            }

            if (!DetectedPlayerService.CheckTargetPositionOnObstacle(_currentTarget))
            {
                StateMachine.ChangeState<PathfinderMove>(); //В будущем заменить на BackstepPathfinderMove после добавления
                return;
            }
            
            BaseMove(_currentTarget, MoveData.BackStepSettings.backstepSpeed);
        }
    }
}