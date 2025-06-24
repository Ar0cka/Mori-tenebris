using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.Stats.Scripts;
using Actors.Player.AttackSystem.Scripts;
using Player.Inventory;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Actors.Player.AttackSystem
{
    public abstract class AttackEnemyAbstract : MonoBehaviour
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

        protected float CurrentAngle;
        protected Vector2 CurrentSize;
        
        protected int MaxComboAttack;
        protected int CurrentCountAttack;
        protected string CurrentAnimationName;
        protected float LastClick;
        protected Coroutine ExitFromComboCoroutine;
        protected Coroutine AttackCoroutine;
        
        protected float Cooldown;
        
        protected Action<SendEquipWeaponEvent> EquipWeaponAction;
        
        protected bool CanAttack =>
            Input.GetMouseButtonDown(0) && Cooldown <= 0 && !PlayerStates.IsBusy;
       
        protected virtual void Awake()
        {
            if (ValidateComponents())
            {
                enabled = false;
                return;
            }
        }

        protected virtual IEnumerator SetAnimation(string animationName, Transform hitPositon,
            WeaponHitColliderSettings hitCollider)
        {
            animator.SetFloat(nameSideParameters, GetAnimationVector());
            animator.SetTrigger(animationName);
            
            yield return new WaitForSeconds(startAnimationDelay);
            
            yield return new WaitForSeconds(checkHitAnimationDelay);
                
            CurrentAngle = GetAnimationVector() == leftSide ? hitCollider.angleColliderLeft : hitCollider.angleColliderRight;
            CurrentSize = hitCollider.hitSize;
            
            var hit = Physics2D.OverlapBoxAll(hitPositon.position, hitCollider.hitSize, CurrentAngle);

            var sortedList = hit.Where(a => a.CompareTag("Enemy")).ToList();
            
            foreach (var currentHit in sortedList)
            {
                EnemyData enemyData = currentHit.GetComponent<EnemyData>();
                enemyData.TakeDamage(PlayerDamageSystem.Damage, PlayerDamageSystem.DamageType);
            }
            
            yield return new WaitForSeconds(endAnimationDelay);
        }

        protected int GetAnimationVector()
        {
            return spriteRenderer.flipX ? leftSide : rightSide;
        }
        
        protected bool ValidateComponents()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (animator == null) animator = GetComponentInParent<Animator>();
            
            
            return spriteRenderer == null || animator == null;
        }

        protected void BaseExit()
        {
            CurrentCountAttack = 0;
            CurrentAnimationName = "";
            ExitFromComboCoroutine = null;
        }
    }
}