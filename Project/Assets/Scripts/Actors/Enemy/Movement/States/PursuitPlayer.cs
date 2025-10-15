using Actors.Enemy.Movement;
using Actors.Enemy.Movement.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class PursuitPlayer : MoveEnemyFsmUnityState
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly EnemyMoveFsmRealize _fsmRealize;
        private MoveSettings _moveSettings;
        private AggressiveSettings _aggressiveSettings;

        private bool _stopDistance;
        private float _timer;

        private const float LingerDistance = 0.5f;

        private Vector2 _lastCheckPoint = Vector2.zero;
        
        public PursuitPlayer(EnemyMoveFsm fsm, Rigidbody2D rigidbody2D,
           EnemyMoveFsmRealize fsmRealize , MoveSettings moveData, AggressiveSettings aggressiveSettings) : base(fsm)
        {
            _rigidbody = rigidbody2D;
            _fsmRealize = fsmRealize;
            _moveSettings = moveData;
            _aggressiveSettings = aggressiveSettings;
        }

        public override void Enter()
        {
            if (!_fsmRealize.OnSeePlayer)
                ChooseIdleState(_moveSettings);

            _timer = 0;
        }

        public override void PhysicsUpdate()
        {
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = _fsmRealize.GetTargetPosition();
            
            _stopDistance = Vector2.Distance(_rigidbody.position, targetPosition) <= _aggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 LingerTime();
                 return;
            }
            
            if (!_stopDistance)
            {
                Vector2 direction = (_fsmRealize.GetTargetPosition() - _rigidbody.position).normalized;
                Vector2 moveDirection = direction * _moveSettings.speed * Time.fixedDeltaTime;
                _rigidbody.MovePosition(_rigidbody.position + moveDirection);
            }
            
            _timer = 0;
        }

        private void LingerTime()
        {
            _timer += Time.deltaTime;
            
            Debug.Log($"Linger time: {_timer}");

            if (_timer >= _aggressiveSettings.lingerTime)
            {
                Debug.Log("Linger change state");
                ChooseIdleState(_moveSettings);
            }

            if (_lastCheckPoint == Vector2.zero ||
                Vector2.Distance(_lastCheckPoint, _rigidbody.position) < LingerDistance)
            {
                _lastCheckPoint = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
                _lastCheckPoint += _rigidbody.position; //Сделать проверку дошел ли до точки
            } 
                
            
            Vector2 moveDirection = (_lastCheckPoint - _rigidbody.position).normalized;
            
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_moveSettings.speed + 1) * Time.fixedDeltaTime);
        }
        
        public override void Exit()
        {
            _fsmRealize.ChangeViewState(false);
            _lastCheckPoint = Vector2.zero;
        }
    }
}