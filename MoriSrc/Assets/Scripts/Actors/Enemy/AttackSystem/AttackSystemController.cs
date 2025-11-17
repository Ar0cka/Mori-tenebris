using System;
using Actors.Enemy.AttackSystem.States;
using Actors.Enemy.Monsters.AbstractEnemy;
using FiniteStateMachine;
using System.Collections.Generic;
using Actors.Enemy.Movement.Base.Service;
using Actors.Enemy.Stats.Scripts.TakeDamageSystem;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.AttackSystem
{
    public abstract class AttackSystemController<TConfig> : FsmRealizeBase<AttackEnemyFsm>
    {
        [SerializeField] protected TConfig config;
        [SerializeField] protected HealthController healthController;
        [SerializeField] protected StateController stateController;
        
        [Inject] private DetectedPlayerService _detectedPlayerService;
        
        protected List<IDisposable> Disposable = new List<IDisposable>();

        /// <summary>
        /// Initialize from method InitializeAttacks() in method Initialize();
        /// </summary>
        protected List<IEnemyAttack> AttacksList {get; private set;}
        
        /// <summary>
        /// Initializes the FSM and the base attack list from the InitializeAttacks method and then calls OnStatesInitialize()
        /// </summary>
        public sealed override void Initialize()
        {
            if (stateController == null || config == null)
                throw new NullReferenceException();
            
            FsmUnityBase = new AttackEnemyFsm();

            AttacksList = InitializeAttacks();
            
            OnStatesInitialize();
        }

        /// <summary>
        /// Base Update cooldown attacks from AttacksList;
        /// </summary>
        protected override void Update()
        {
            foreach (var attack in AttacksList)
            {
                attack.CooldownAttack();
            }
        }

        /// <summary>
        /// Override this method for change states in your FSM
        /// Initializes states: Idle, Attack, Interrupt and enter to Idle after initialized.
        /// </summary>
        protected virtual void OnStatesInitialize()
        {
            FsmUnityBase.AddState(CreateIdleState());
            FsmUnityBase.AddState(CreateInterruptState());
            FsmUnityBase.AddState(CreateAttackState());
            
            FsmUnityBase.Idle();
        }

        /// <summary>
        /// Override this method so that the attack states get the desired list of attacks.
        /// </summary>
        /// <returns>List with IEnemyAttack</returns>
        protected abstract List<IEnemyAttack> InitializeAttacks();

        /// <summary>
        /// Override for change default Attack state. Use for reference FsmUnityBase and AttackList
        /// </summary>
        /// <returns>Default type = Actors.Enemy.AttackSystem.States.AttackState</returns>
        protected virtual AttackState CreateAttackState() => new AttackState(FsmUnityBase, AttacksList);
        /// <summary>
        /// Override for change default interrupt type. Use for reference FSMUnityBase and StateController.
        /// </summary>
        /// <returns>Default type = Actors.Enemy.AttackSystem.States.Interrupt</returns>
        protected virtual Interrupt CreateInterruptState() => new Interrupt(FsmUnityBase, stateController);
        /// <summary>
        /// Override for change default idle type. Use for reference data TConfig and InitializeAttacks and HealthController for connect to event and save disponse for unsubscribe
        /// </summary>
        /// <returns>Default type = Actors.Enemy.AttackSystem.States.AttackIdle</returns>
        protected virtual AttackIdle CreateIdleState()
        {
            var idleState = new AttackIdle(FsmUnityBase, AttacksList);
            var disposable = healthController.SubscribeNewObserver(idleState);
            Disposable.Add(disposable);
            return idleState;
        }

        protected virtual void OnDestroy()
        {
            foreach (var item in Disposable)
            {
                item.Dispose();
            }
            
            Disposable.Clear();
        }
    }
}