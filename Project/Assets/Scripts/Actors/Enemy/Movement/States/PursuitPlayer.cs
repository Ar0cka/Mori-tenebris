using Actors.Enemy.Movement;
using Actors.Enemy.Movement.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class PursuitPlayer : BaseMovementState
    {
        private AggressiveSettings _aggressiveSettings;

        private bool _stopDistance;
        private float _timer;

        private const float LingerDistance = 0.5f;

        private Vector2 _lastCheckPoint = Vector2.zero;
        
        public PursuitPlayer(EnemyMoveFsm fsm, BaseMovementContext context, 
           AggressiveSettings aggressiveSettings) : base(fsm, context)
        {
            _aggressiveSettings = aggressiveSettings;
        }

        public override void Enter()
        {
            if (!FsmRealize.OnSeePlayer)
                ChooseIdleState(MoveSettings);
            
            base.Enter();

            _timer = 0;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = FsmRealize.GetTargetPosition();
            
            _stopDistance = Vector2.Distance(Rb2D.position, targetPosition) <= _aggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 LingerTime();
                 return;
            }
            
            if (!_stopDistance)
            {
                BaseMove(FsmRealize.GetTargetPosition(), MoveSettings.speed);
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
                ChooseIdleState(MoveSettings);
            }

            if (_lastCheckPoint == Vector2.zero ||
                Vector2.Distance(_lastCheckPoint, Rb2D.position) < LingerDistance)
            {
                _lastCheckPoint = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
                _lastCheckPoint += Rb2D.position; //Сделать проверку дошел ли до точки
            } 
                
            
            Vector2 moveDirection = (_lastCheckPoint - Rb2D.position).normalized;
            
            Rb2D.MovePosition(Rb2D.position + moveDirection * (MoveSettings.speed + 1) * Time.fixedDeltaTime);
        }
        
        public override void Exit()
        {
            FsmRealize.ChangeViewState(false);
            _lastCheckPoint = Vector2.zero;
        }
    }
}