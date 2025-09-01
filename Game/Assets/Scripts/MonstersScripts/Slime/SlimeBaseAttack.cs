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
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBaseAttack : EnemyAttackBase
{
    [SerializeField] private EffectScrObj poisonEffect;
    private Dictionary<int, AnimAttackSettings> attackSequence = new Dictionary<int, AnimAttackSettings>();
    
    public void InitializeAttack(EnemyDamage damageSystem, StateController stateController, Transform playerTransform)
    {
        InitializeComponents(damageSystem, stateController, playerTransform);

        if (_currentAttackConfig.nameAttack == attackName)
        {
            var sortedAttacks = _currentAttackConfig.animAttackSettings
                .OrderBy(a => a.countInQueue)
                .ToList();
        
            foreach (var attack in sortedAttacks)
            {
                attackSequence.TryAdd(attack.countInQueue, attack);
            }
        }
    }

    public override float BeginAttack()
    {
        float x = spriteRenderer.flipX ? 0 : 1;
        
        PlayAttackAnimation(attackSequence[CurrentCountAttack], x);
        
        return attackSequence[CurrentCountAttack].startAttackDelay;
    }

    public override float ExecuteHit()
    {
        GameObject hitObject = null;
        
        foreach (var tag in hittableTags)
        {
            hitObject = HitSystem.CircleHit(hitOrigin.position, attackRadius, tag);
            
            if (hitObject != null)
                break;
        }

        if (hitObject == null) return _currentAttackConfig.animAttackSettings[CurrentCountAttack].hitDelay;
        
        ITakeDamage takeDamage = hitObject.GetComponentInChildren<ITakeDamage>();
        
        ApplyDamageWithEffect(takeDamage, poisonEffect);
        
        return attackSequence[CurrentCountAttack].hitDelay;
    }

    public override bool EndAttack()
    {
        CurrentCountAttack++;
        
        if (CurrentCountAttack < MaxCountAttack)
        {
            return false;
        }
        
        if (_exitCoroutine == null) 
            StartCoroutine(ExitComboCoroutine());
        
        return true;
    }

    public override bool IsTargetInRange() => IsDistanceLess(attackConfig.GetAttackConfig().attackDistance);

    public override IEnumerator ExitComboCoroutine()
    {
        yield return StartCoroutine(ExitFromComboCoroutine());
    }
    
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hitOrigin.position, attackRadius);
    }

#endif
}
