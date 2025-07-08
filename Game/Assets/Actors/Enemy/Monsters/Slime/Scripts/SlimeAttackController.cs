using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.AttackSystem.Scripts;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using UnityEngine;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeAttackController : MonstersBattleController
    {
        [SerializeField] private SlimeBaseAttack slimeBaseAttack;
        [SerializeField] private SlimeJumpAttack slimeJumpAttack;

        private List<EnemyAttackBase> _attackStates = new();
        private SlimeScrObj _monsterData;

        private SlimeConfig _slimeConfig;
        private AttackAction _attackAction;

        public override void Initialize()
        {
            base.Initialize();

            if (MonsterScrObj is SlimeScrObj slimeScr)
                _monsterData = slimeScr;
            
            if (_monsterData.GetConfig() is SlimeConfig slimeConfig)
            {
                _slimeConfig = slimeConfig;
            }

            PlayerTransform = enemyData.PlayerPosition;

            slimeBaseAttack.InitializeAttack(enemyData.GetDamageSystem(), StateController, PlayerTransform);
            slimeJumpAttack.Initialize(enemyData.GetDamageSystem(), _slimeConfig, StateController, PlayerTransform);
            
            _attackStates.Add(slimeBaseAttack);
            _attackStates.Add(slimeJumpAttack);

            IsInitialize = true;
            
            Debug.Log("Initialize attack controller");
        }

        protected override void FixedUpdate()
        {
            if (!IsInitialize) return;

            if (!IsHaveState && StateController.CanAttack())
            {
                EnemyAttackBase currentAttack = ChoiceAttackState();
                AttackAction.EnterInAttackState(currentAttack);
            }
            
            if (IsHaveState) AttackAction.Tick();
        }

        private EnemyAttackBase ChoiceAttackState()
        {
            EnemyAttackBase bestAttack = null;

            foreach (var state in _attackStates)
            {
                if (state.IsOnCooldown || !state.IsTargetInRange())
                    continue;

                if (bestAttack == null || bestAttack.GetWeight() < state.GetWeight())
                    bestAttack = state;
            }

            Debug.Log("Get attack: " + bestAttack?.name);
            
            return bestAttack;
        }
    }

    public class AttackAction
    {
        private MonstersBattleController _monsterController;

        private EnemyAttackBase _enemyAttackBase;

        private float _stateDelay;
        private AttackStates _currentState;

        public AttackAction(MonstersBattleController battleController)
        {
            _monsterController = battleController;
        }

        public void EnterInAttackState(EnemyAttackBase enemyAttackBase)
        {
            if (enemyAttackBase == null) return;
            
            _monsterController.ChangeAttackState(true);
            _enemyAttackBase = enemyAttackBase;

            _stateDelay = _enemyAttackBase.BeginAttack();
            _currentState = AttackStates.StartAttack;
        }

        public void Tick()
        {
            if (_currentState == AttackStates.Idle) return;
            
            _stateDelay -= Time.deltaTime;
            if (_stateDelay > 0) return;

            switch (_currentState)
            {
                case AttackStates.StartAttack:
                    _stateDelay = _enemyAttackBase.ExecuteHit();
                    _currentState = AttackStates.Action;
                    break;
                case AttackStates.Action:
                    bool canEnd = _enemyAttackBase.EndAttack();
                    if (!canEnd)
                    {
                        _stateDelay = _enemyAttackBase.BeginAttack();
                        _currentState = AttackStates.StartAttack;
                    }
                    else
                    {
                        Exit();
                    }
                    break;
            }
        }

        private void Exit()
        {
            _monsterController.ChangeAttackState(false);
        }
    }
}

public enum AttackStates
{
    Idle,
    StartAttack,
    Action
}