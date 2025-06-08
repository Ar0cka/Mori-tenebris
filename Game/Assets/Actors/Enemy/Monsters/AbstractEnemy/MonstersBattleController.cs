using System;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Movement;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public abstract class MonstersBattleController : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected EnemyMove enemyMove;
        [SerializeField] protected EnemyData enemyData;

        protected MonsterScrObj MonsterScrObj;
        
        protected Transform PlayerTransform;

        public virtual void Initialize()
        {
            if (OnValidateComponents())
            {
                enabled = false;
                return;
            }

            MonsterScrObj = enemyData.GetEnemyScrObj();
        }
        
        protected bool OnValidateComponents()
        {
            
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (enemyMove == null) enemyMove = GetComponentInChildren<EnemyMove>();
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
        
        protected bool CheckDistance(float distance)
        {
            return Vector2.Distance(transform.position, PlayerTransform.position) < distance ;
        }
    }
}