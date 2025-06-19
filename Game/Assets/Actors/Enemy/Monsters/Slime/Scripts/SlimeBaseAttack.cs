using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.AttackSystem.Scripts;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using Enemy.StatSystems.DamageSystem;
using HitChecker;
using NegativeEffects;
using PlayerNameSpace;
using UnityEngine;

public class SlimeBaseAttack : EnemyAttackBase
{
    [SerializeField] private EffectScrObj poisonEffect;
    
    private SlimeConfig slimeConfig;
    
    private Dictionary<int, AnimAttackSettings> attackSequence = new Dictionary<int, AnimAttackSettings>();
    
    private float lastAttackTimestamp;
    
    public void InitializeAttack(EnemyDamage damageSystem, 
        AttackConfig attackConfig, SlimeConfig config, StateController stateController, Transform playerTransform)
    {
        InitializeComponents(damageSystem, stateController);
        
        if (damageSystem != null && config != null && attackConfig != null)
        {
            slimeConfig = config;
            _currentAttackConfig = attackConfig;
        }

        if (_currentAttackConfig.nameAttack == attackName)
        {
            var sortedAttacks = _currentAttackConfig.animAttackSettings
                .OrderBy(a => a.countInQueue)
                .ToList();
        
            foreach (var attack in sortedAttacks)
            {
                attackSequence.TryAdd(attack.countInQueue, attack);
            }

            _maxComboCount = attackSequence.Count;
            
            _playerTransform = playerTransform;
        }
    }

    protected override void Update()
    {
        if (Time.time - lastAttackTimestamp > comboInputWindow)
        {
            ExitCombo();
        }
        
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public override void TryAttack()
    {
        if (attackSequence.TryGetValue(_currentComboCount, out var currentAttack))
        {
            if (CanPerformAttack())
            {
                StartCoroutine(PerformAttack(currentAttack.startAttackDelay, currentAttack.hitDelay, currentAttack));
            }
            else if (!IsPlayerInRange(_currentAttackConfig.attackDistance))
            {
                animator.ResetTrigger(currentAttack.nameTrigger);
                _exitCoroutine = StartCoroutine(ExitComboCoroutine());
            }
        }
        
        ExitCombo();
    }

    protected override bool BeginAttack(AnimAttackSettings attackSettings)
    {
        if (attackSettings == null) return false;

        float x = spriteRenderer.flipX ? 0 : 1;

        _currentComboCount++;
        
        PlayAttackAnimation(attackSettings, x);
        
        return true;
    }

    protected override bool ExecuteHit()
    {
        GameObject hitObject = null;
        
        foreach (var tag in hittableTags)
        {
            hitObject = HitSystem.CircleHit(hitOrigin.position, attackRadius, tag);
            
            if (hitObject != null)
                break;
        }

        if (hitObject == null) return false;
        
        ITakeDamage takeDamage = hitObject.GetComponentInChildren<ITakeDamage>();
        
        var hitSuccess = ApplyDamageWithEffect(takeDamage, poisonEffect);
        
        return hitSuccess;
    }

    protected override void EndAttack()
    {
        _exitCoroutine = StartCoroutine(ExitComboCoroutine());
    }

    private void ExitCombo()
    {
        if (_currentComboCount >= _maxComboCount)
        {
            if (_exitCoroutine == null)
            {
                Debug.Log("Reset combo");
                _exitCoroutine = StartCoroutine(ExitComboCoroutine());
            }
        }
    }

    private IEnumerator ExitComboCoroutine()
    {
        ResetAttackCooldown(_currentAttackConfig.cooldownAttack);
        
        yield return new WaitForSeconds(attackExitDelay);
        
        foreach (var attackEntry in attackSequence)
        {
            animator.ResetTrigger(attackEntry.Value.nameTrigger);
        }
        
        ExitAttack();
    }

    private bool CanPerformAttack()
    {
        return attackCooldown <= 0 && IsPlayerInRange(_currentAttackConfig.attackDistance) &&
               _stateController.CanAttack() && _currentComboCount < _maxComboCount;
    }
    
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hitOrigin.position, attackRadius);
    }

#endif
}
