using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    public class RangeAiMoveFsmRealize : EnemyMoveFsmRealize
    {
        [SerializeField] private RangeMoveData _rangeData;
        protected override MoveData MoveData => _rangeData;

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}