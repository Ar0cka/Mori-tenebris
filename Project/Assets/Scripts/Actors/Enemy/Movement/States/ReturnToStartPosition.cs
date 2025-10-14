using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class ReturnToStartPosition : MoveEnemyFsmUnityState
    {
        private readonly EnemyMoveFsmRealize _fsmRealize;
        private readonly Rigidbody2D _rb2D;
        private readonly Vector2 _startPosition;
        private readonly MoveSettings _moveSettings;

        private bool _hasArrived = false;
        private float _distance = 0.5f;
        
        public ReturnToStartPosition(EnemyMoveFsm fsm, EnemyMoveFsmRealize fsmRealize, Rigidbody2D rb2D, 
            Vector2 startPosition, MoveSettings moveSettings) : base(fsm)
        {
            _fsmRealize = fsmRealize;
            _rb2D = rb2D;
            _startPosition = startPosition;
            _moveSettings = moveSettings;
        }

        public override void Enter()
        {
            _hasArrived = false;
            Debug.Log("Entering ReturnToStartPosition");
        }

        public override void PhysicsUpdate()
        {
            if (_hasArrived) return;
            
            if (_fsmRealize.DetectTarget())
                StateMachine.ChangeState<PursuitPlayer>();
            
            Vector2 direction = (_startPosition - _rb2D.position).normalized;

            // Простое возвращение к старту
            _rb2D.MovePosition(_rb2D.position + direction * _moveSettings.speed * Time.deltaTime);

            // Проверка прибытия
            if (Vector2.Distance(_rb2D.position, _startPosition) <= _distance)
            {
                _hasArrived = true;
                Debug.Log($"Returning To StartPosition {_startPosition} where current position is {_rb2D.position}");
                
                ChooseIdleState(_moveSettings);
            }
        }

        public override void Exit()
        {
            _hasArrived = false;
        }
    }
}