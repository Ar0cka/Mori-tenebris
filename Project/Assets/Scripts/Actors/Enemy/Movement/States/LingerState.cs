using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class LingerState : BaseMovementState
    {
        private float _timer;
        private Vector2 _lastCheckPoint;
        private readonly AggressiveSettings _aggressiveSettings;
        
        public LingerState(EnemyMoveFsm fsm, BaseMovementContext context, 
            AggressiveSettings aggressiveSettings) : base(fsm, context)
        {
            _aggressiveSettings = aggressiveSettings;
        }

        public override void Enter()
        {
            base.Enter();

            _timer = 0;
            _lastCheckPoint = Vector2.zero;
        }

        public override void PhysicsUpdate()
        {
            Vector2 targetPos = FsmRealize.GetTargetPosition();
            
            Debug.Log(targetPos + " Player position");
            
            if (targetPos != Vector2.zero)
                StateMachine.ChangeState<PursuitPlayer>();
            
            LingerTime();
        }

        private void LingerTime()
        {
            _timer += Time.deltaTime;
            if (_timer >= _aggressiveSettings.lingerTime)
            {
                Debug.Log("Linger change state");
                ChooseIdleState(MoveSettings);
            }

            if (_lastCheckPoint == Vector2.zero ||
                Vector2.Distance(_lastCheckPoint, Rb2D.position) < _aggressiveSettings.lingerDistance)
            {
                _lastCheckPoint = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
                _lastCheckPoint += Rb2D.position; //Сделать проверку дошел ли до точки
            } 
                
            
            BaseMove(_lastCheckPoint, MoveSettings.speed + 1);
        }
    }
}