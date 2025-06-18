using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using UnityEngine;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeAttackController : MonstersBattleController
    {
        [SerializeField] private SlimeSpriteConroller slimeSpriteConroller;
        [SerializeField] private SlimeBaseAttack slimeBaseAttack;
        [SerializeField] private SlimeJumpAttack slimeJumpAttack;
        [SerializeField] private float cooldown;

        [SerializeField] private float baseAttackDistance;
        [SerializeField] private float jumpMax;
        [SerializeField] private float jumpMin;

        private SlimeConfig _slimeConfig;
        private List<AttackConfig> _attackConfig;

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
                    
                PlayerTransform = enemyData.PlayerPosition;
                
                slimeBaseAttack.InitializeAttack(enemyData.GetDamageSystem(),
                    GetAttackConfigFromList(slimeBaseAttack.AttackName), _slimeConfig, _stateController, PlayerTransform);
                
                slimeJumpAttack.Initialize(enemyData.GetDamageSystem(), GetAttackConfigFromList(slimeJumpAttack.AttackName), _slimeConfig, _stateController, PlayerTransform);
            }
            
            _isInitialize = true;
        }

        private void Update()
        {
            if (!_isInitialize || !_stateController.CanAttack()) return;
            
            RotateMonster(slimeSpriteConroller);
            
            if (CheckJumpDistance(jumpMin, jumpMax) && !slimeJumpAttack.IsOnCooldown)
            {
                slimeJumpAttack.TryAttack();
            }
            else if (CheckDistance(baseAttackDistance) && !slimeBaseAttack.IsOnCooldown)
            {
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

        private bool CheckJumpDistance(float minDistance, float maxDistance)
        {
            return Vector2.Distance(transform.position, PlayerTransform.position) < maxDistance && 
                   Vector2.Distance(transform.position, PlayerTransform.position) > minDistance;
        }
    }
}