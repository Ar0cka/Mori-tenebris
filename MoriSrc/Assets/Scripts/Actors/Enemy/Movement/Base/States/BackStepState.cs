using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class BackStepState : BaseMovementState<MileEnemyMoveRealize, MoveData>
    {
        protected Vector2 _currentTarget;
        protected BackStepSettings _backStepSettings;
        
        public BackStepState(EnemyMoveFsm fsm, DataContext<MileEnemyMoveRealize, MoveData> dataContext, 
            BaseMovementContext context) : base(fsm, dataContext, context)
        {
            _backStepSettings = MoveConfig.BackStepSettings;
        }

        public override void PhysicsUpdate()
        {
            Backstep();
        }

        protected virtual void Backstep()
        {
            Vector2 objectPosition = DetectedPlayer(MoveConfig);
            
            if (Vector2.Distance(Rb2D.position, objectPosition) <= _backStepSettings.maxDistance)
            {
                StateMachine.ChangeState<IdleMoveState>(); //В будущем сменить на Idle атаки либо иные виды, например регенирация
            }
            
            if (_currentTarget == Vector2.zero || Vector2.Distance(Rb2D.position, _currentTarget) <= _backStepSettings.distanceForChangeVector)
            {
                if (objectPosition == Vector2.zero)
                {
                    ChooseIdleState(MoveConfig.MoveSettings);
                    return;
                }
                
                _currentTarget = Rb2D.position - objectPosition;
                
                Vector2 offset = _backStepSettings.backStepOffset;
                
                _currentTarget += new Vector2(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y));
                _currentTarget = _currentTarget.normalized;
            }

            if (!DetectedPlayerService.CheckTargetPositionOnObstacle(_currentTarget))
            {
                StateMachine.ChangeState<PathfinderMove>(); //В будущем заменить на BackstepPathfinderMove после добавления
                return;
            }
            
            BaseMove(_currentTarget, _backStepSettings.backstepSpeed);
        }
    }
}