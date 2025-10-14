using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.MovementFsm.States
{
    public class PursuitPlayer : MoveEnemyFsmUnityState
    {
        private Rigidbody2D _rigidbody;
        private AggressiveSettings _aggressiveSettings;
        
        public PursuitPlayer(EnemyMoveFsm fsm, Rigidbody2D rigidbody2D,
            AggressiveSettings agrData) : base(fsm)
        {
            _rigidbody = rigidbody2D;
            _aggressiveSettings = agrData;
        }
    }
}