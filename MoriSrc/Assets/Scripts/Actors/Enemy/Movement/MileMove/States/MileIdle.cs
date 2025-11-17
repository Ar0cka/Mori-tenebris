using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement.Base.States
{
    public class MileIdle : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        public MileIdle(BaseMovementContext<MoveData, EnemyMoveFsm> context) : base(context)
        {
            
        }
        
        public override void Update()
        {
            bool isDetected = IdleDetected();

            if (isDetected)
            { 
                Ctx.Fsm.ChangeState<PursuitPlayer>();
            }
        }

        protected virtual bool IdleDetected()
        {
            var idleConfig = Ctx.Config.IdleDetectionSettings;

            return Ctx.DetectedPlayerService.IdleDetection(idleConfig.idleDetectionRadius, idleConfig.fieldOfViewAngle,
                Ctx.Config.TargetMask, Ctx.SpriteRenderer, Ctx.Rb2D.position);
        }
    }
    
}