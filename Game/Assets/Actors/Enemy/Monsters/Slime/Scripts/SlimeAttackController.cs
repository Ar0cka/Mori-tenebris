using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using PlayerNameSpace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeAttackController : MonstersBattleController
    {
        [SerializeField] private SlimeSpriteConroller slimeSpriteConroller;
        [SerializeField] private SlimeBaseAttack slimeBaseAttack;
        [SerializeField] private float cooldown;

        [SerializeField] private float baseAttackDistance;
        [SerializeField] private float jumpAttackDistance;

        private SlimeConfig _slimeConfig;
        private List<AttackConfig> _attackConfig;

        private Transform _playerTransform;
        private float _cooldown;

        private float maxDistance;

        private bool _isInitialize;
        
        private StateController _stateController;

        public override void Initialize()
        {
            base.Initialize();

            if (MonsterScrObj != null)
            {
                if (MonsterScrObj.GetConfig() is SlimeConfig slimeConfig)
                {
                    _slimeConfig = slimeConfig;
                }

                _attackConfig = MonsterScrObj.GetAttackConfig();

                _stateController = enemyData.GetStateController();
                
                slimeBaseAttack.InitializeAttack(enemyData.GetDamageSystem(),
                    GetAttackConfigFromList(slimeBaseAttack.AttackName), _slimeConfig, _stateController);
            }

            _playerTransform = enemyData.PlayerPosition;

            _isInitialize = true;
        }

        private void Update()
        {
            if (!_isInitialize) return;
            
            if (CheckDistance(baseAttackDistance) && _stateController.CanAttack())
            {
                RotateMonster();
                slimeBaseAttack.TryAttack();
            }
                
        }

        private AttackConfig GetAttackConfigFromList(string nameAttack)
        {
            for (int i = 0; i < _attackConfig.Count; i++)
            {
                if (_attackConfig[i].nameAttack == nameAttack)
                {
                    return _attackConfig[i];
                }
            }

            return null;
        }

        
        private void RotateMonster()
        {
            Vector2 rotateVector2 =  (_playerTransform.position - transform.position).normalized;
            slimeSpriteConroller.SetFlipState(rotateVector2);
        }
        
        private bool CheckDistance(float distance)
        {
            return Vector2.Distance(transform.position, _playerTransform.position) < distance;
        }
    }
}