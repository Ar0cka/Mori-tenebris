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
    private Dictionary<int, AnimAttackSettings> attackSequence = new Dictionary<int, AnimAttackSettings>();
    
    public void InitializeAttack(EnemyDamage damageSystem, StateController stateController, Transform playerTransform)
    {
        InitializeComponents(damageSystem, stateController);

        if (_currentAttackConfig.nameAttack == attackName)
        {
            var sortedAttacks = _currentAttackConfig.animAttackSettings
                .OrderBy(a => a.countInQueue)
                .ToList();
        
            foreach (var attack in sortedAttacks)
            {
                attackSequence.TryAdd(attack.countInQueue, attack);
            }
            
            _playerTransform = playerTransform;
        }
    }

    public override bool BeginAttack(AnimAttackSettings attackSettings)
    {
        if (attackSettings == null) return false;

        float x = spriteRenderer.flipX ? 0 : 1;
        
        PlayAttackAnimation(attackSettings, x);
        
        return true;
    }

    public override bool ExecuteHit()
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

    public override void EndAttack()
    {
        
        
        if (_exitCoroutine == null) 
            _exitCoroutine = StartCoroutine(ExitComboCoroutine());
    }

    public override bool IsTargetInRange() => IsDistanceLess(attackConfig.GetAttackConfig().attackDistance);

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
    
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hitOrigin.position, attackRadius);
    }

#endif
}
