using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement.Base.States
{
    public class PathfinderMove : MoveEnemyFsmUnityState
    {
        public PathfinderMove(EnemyMoveFsm enemyMoveFsm, BaseMovementContext context) : base(enemyMoveFsm, context)
        {
            
        }
    }
}