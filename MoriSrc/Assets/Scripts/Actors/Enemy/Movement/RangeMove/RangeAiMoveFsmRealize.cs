using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.RangeMove.States;
using Actors.Enemy.Movement.RangeMove.States;
using Actors.Enemy.Movement.Service;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Movement.RangeMove
{
    public class RangeAiMoveFsmRealize : EnemyMoveFsmRealize
    {
        [SerializeField] private RangeMoveData rangeData;
        protected override MoveData MoveData => rangeData;
        
        public RadiusService<RadiusSettings> RadiusService { get; private set; }

        public override void Initialize()
        {
            RadiusService = new RadiusService<RadiusSettings>(rangeData.RadiusSettings);
            
            base.Initialize();
            
            BaseMovementContext baseMovementContext = CreateMovementContext();
            
            StatesInit(this, baseMovementContext);
        }

        protected override void StatesInit(EnemyMoveFsmRealize moveFsmRealize, BaseMovementContext baseMovementContext)
        {
            base.StatesInit(moveFsmRealize, baseMovementContext);

            var dataContext = new DataContext<RangeAiMoveFsmRealize, RangeMoveData>(rangeData, this);
            
            FsmUnityBase.AddState(new MediumRadiusState(FsmUnityBase, dataContext, baseMovementContext, RadiusService));
        }
    }
}