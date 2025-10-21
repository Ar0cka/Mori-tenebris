using System.Collections.Generic;
using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement
{
    public class MileEnemyMoveRealize : EnemyMoveFsmRealize<MoveData>
    {
        public override void Initialize()
        {
            base.Initialize();
            
            var dataContext = new DataContext<EnemyMoveFsmRealize<MoveData>, MoveData>(moveData, this);
            
            StatesInit(dataContext);

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