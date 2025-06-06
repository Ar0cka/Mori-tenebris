using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.Stats.Scripts;
using Actors.Player.AttackSystem.Scripts;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Actors.Player.AttackSystem
{
    public class AbstractAttack : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        [Header("SidesValue")] 
        [SerializeField] protected string nameSideParameters;
        [SerializeField] protected int leftSide;
        [SerializeField] protected int rightSide;

        [Header("Control animation")]
        [SerializeField] protected float startAnimationDelay;
        [SerializeField] protected float checkHitAnimationDelay;
        [SerializeField] protected float endAnimationDelay;
        
        [Inject] protected DamageSystem PlayerDamageSystem;
        
        protected Dictionary<int, AttackData> _queueAttackDictionary = new Dictionary<int, AttackData>();
        
        protected float Cooldown;
        protected string CurrentAnimation;
        protected bool isBusy;
       
        protected virtual void Awake()
        {
            if (ValidateComponents())
            {
                enabled = false;
                return;
            }
        }

        protected virtual IEnumerator SetAnimation(string animationName, Transform hitPositon, Vector2 sizeHitCollider,
            float angle)
        {
            animator.SetFloat(nameSideParameters, GetAnimationVector());
            animator.SetTrigger(animationName);
            
            yield return new WaitForSeconds(startAnimationDelay);
            
            yield return new WaitForSeconds(checkHitAnimationDelay);
            
            var hit = Physics2D.OverlapBoxAll(hitPositon.position, sizeHitCollider, angle);

            var sortedList = hit.Where(a => a.CompareTag("Enemy")).ToList();
            
            foreach (var currentHit in sortedList)
            {
                EnemyData enemyData = currentHit.GetComponent<EnemyData>();
                enemyData.TakeDamage(PlayerDamageSystem.Damage, PlayerDamageSystem.DamageType);
            }
            
            yield return new WaitForSeconds(endAnimationDelay);
        }

        private int GetAnimationVector()
        {
            return spriteRenderer.flipX ? leftSide : rightSide;
        }
        
        protected bool ValidateComponents()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (animator == null) animator = GetComponentInParent<Animator>();
            
            
            return spriteRenderer == null || animator == null;
        }
    }
}