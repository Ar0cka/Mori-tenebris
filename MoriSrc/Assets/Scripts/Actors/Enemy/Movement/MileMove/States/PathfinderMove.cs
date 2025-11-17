using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement.Base.States
{
    public class PathfinderMove : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        public PathfinderMove(BaseMovementContext<MoveData, EnemyMoveFsm> context) : base(context)
        {
            
        }
    }
}