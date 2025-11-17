using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class LingerState : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        private float _timer;
        private Vector2 _lastCheckPoint;
        
        public LingerState(BaseMovementContext<MoveData, EnemyMoveFsm> context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _timer = 0;
            _lastCheckPoint = Vector2.zero;
        }

        public override void PhysicsUpdate()
        {
            Vector2 targetPos = DetectedPlayer(Ctx.Config.AggressiveSettings.detectionRadius, Ctx.Config.TargetMask);
            
            Debug.Log(targetPos + " Player position");
            
            if (targetPos != Vector2.zero)
                Ctx.Fsm.ChangeState<PursuitPlayer>();
            
            LingerTime();
        }

        private void LingerTime()
        {
            _timer += Time.deltaTime;
            if (_timer >= Ctx.Config.AggressiveSettings.lingerTime)
            {
                Debug.Log("Linger change state");
                ChooseIdleState<MileIdle, PatrolMoveState>(Ctx.Config.MoveSettings);
            }

            if (_lastCheckPoint == Vector2.zero ||
                Vector2.Distance(_lastCheckPoint, Ctx.Rb2D.position) < Ctx.Config.AggressiveSettings.lingerDistance)
            {
                _lastCheckPoint = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
                _lastCheckPoint += Ctx.Rb2D.position; //Сделать проверку дошел ли до точки
            } 
                
            
            BaseMove(_lastCheckPoint, Ctx.Config.MoveSettings.speed + 1);
        }
    }
}