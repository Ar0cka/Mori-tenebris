using System;
using System.Collections.Generic;
using Actors.Enemy.Movement.Base.Service;
using Actors.Enemy.Movement.Base.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement.Base
{
    public class MileEnemyMoveRealize : EnemyMoveFsmRealize
    {
        [SerializeField] private MoveData moveData;
        protected override MoveData MoveData => moveData;

        public override void Initialize()
        {
            base.Initialize();
            
            StatesInit();

            if (moveData.MoveSettings.hasPatrol)
            {
                FsmUnityBase.ChangeState<PatrolMoveState>();
            }
            else
            {
                FsmUnityBase.ChangeState<MileIdle>();
            }

            Initialized = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawGizmos();
        }
#endif
        
    }
}