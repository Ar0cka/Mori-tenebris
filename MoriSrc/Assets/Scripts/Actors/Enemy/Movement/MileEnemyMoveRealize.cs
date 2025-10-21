using System.Collections.Generic;
using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement
{
    public class MileEnemyMoveRealize : EnemyMoveFsmRealize
    {
        [SerializeField] private MoveData moveData;
        protected override MoveData MoveData => moveData;

        public override void Initialize()
        {
            base.Initialize();
            
            StatesInit(this);

            if (moveData.MoveSettings.hasPatrol)
            {
                FsmUnityBase.ChangeState<PatrolMoveState>();
            }
            else
            {
                FsmUnityBase.ChangeState<IdleMoveState>();
            }
        }
    }
}