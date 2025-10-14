using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement.MovementFsm.States
{
    public class PatrolMoveState : MoveEnemyFsmUnityState
    {
        private PatrolSettings _patrolSettings;
        private IdleDetectionSettings _idleDetectionSettings;
        
        public PatrolMoveState(EnemyMoveFsm fsm, 
            PatrolSettings patrolSettings, IdleDetectionSettings detectionSettings) 
            : base(fsm)
        {
            _patrolSettings = patrolSettings;
            _idleDetectionSettings = detectionSettings;
        }
    }
}