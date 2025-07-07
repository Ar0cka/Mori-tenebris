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

        [SerializeField] private List<string> attackNames;

        private List<EnemyAttackBase> _attackStates = new();
        private SlimeScrObj _monsterData;

        private SlimeConfig _slimeConfig;
        private AttackAction _attackAction;
        
        public override void Initialize()
        {
            base.Initialize();
            
            if (_monsterData.GetConfig() is SlimeConfig slimeConfig)
            {
                _slimeConfig = slimeConfig;
            }
            
            slimeBaseAttack.InitializeAttack(enemyData.GetDamageSystem(), StateController, PlayerTransform);
            slimeJumpAttack.Initialize(enemyData.GetDamageSystem(), _slimeConfig, StateController, PlayerTransform);
            
            IsInitialize = true;
        }

        protected override void FixedUpdate()
        {
            if (!IsInitialize) return;
            
            if (!IsHaveState)
            {
                EnemyAttackBase currentAttack = ChoiceAttackState();
                
                AttackAction.EnterInAttackState(currentAttack);
            }
        }

        protected EnemyAttackBase ChoiceAttackState()
        {
            EnemyAttackBase bestAttack = null;

            foreach (var state in _attackStates)
            {
                if (state.IsOnCooldown || !state.IsTargetInRange())
                    continue;

                if (bestAttack == null || bestAttack.GetWeight() < state.GetWeight())
                    bestAttack = state;
            }
            
            return bestAttack;
        }
    }

    public class AttackAction
    {
        private MonstersBattleController _monsterController;
        
        private int _maxComboCount;
        private int _currentComboCount;
        
        private AttackConfig _attackConfig;
        private EnemyAttackBase _enemyAttackBase;

        public AttackAction(MonstersBattleController battleController)
        {
            _monsterController = battleController;
        }

        public void EnterInAttackState(EnemyAttackBase enemyAttackBase)
        {
            _attackConfig = enemyAttackBase.GetAttackConfig();
            _enemyAttackBase = enemyAttackBase;

            _maxComboCount = _attackConfig.animAttackSettings.Count;
            _currentComboCount = 0;
        }

        public void UpdateLogic()
        {
            
        }
    }
}