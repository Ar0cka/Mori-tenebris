using System;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.Slime;
using Actors.Enemy.Movement;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public abstract class MonstersBattleController : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected EnemyData enemyData;

        protected MonsterScrObj MonsterScrObj;
        protected Transform PlayerTransform;
        protected StateController StateController;
        protected AttackAction AttackAction;
        
        protected bool IsHaveState;
        protected bool IsInitialize;

        public virtual void Initialize()
        {
            if (OnValidateComponents())
            {
                enabled = false;
                return;
            }

            MonsterScrObj = enemyData.GetEnemyScrObj();
            StateController = enemyData.GetStateController();
            AttackAction = new AttackAction(this);
        }

        protected abstract void FixedUpdate();
        
        protected bool OnValidateComponents()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (enemyData == null) enemyData = GetComponent<EnemyData>();
            

            if (spriteRenderer == null || MonsterScrObj == null)
                return false;
            
            return true;
        }
        protected void RotateMonster(SpriteController spriteController)
        {
            Vector2 rotateVector2 =  (PlayerTransform.position - transform.position).normalized;
            spriteController.SetFlipState(rotateVector2);
        }
        
        public void ChangeAttackState(bool state)
        {
            IsHaveState = state;
        }
    }
}