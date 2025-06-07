using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.AttackSystem.Scripts;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using Actors.Enemy.Movement;
using Enemy.StatSystems.DamageSystem;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBaseAttack : AttackEnemyAbstract
{
    private SlimeConfig _slimeConfig;
    
    private Dictionary<int, AnimAttackSettings> _attackQueue = new Dictionary<int, AnimAttackSettings>();
    
    private float lastAttackTime;
    
    public void InitializeAttack(EnemyDamage damageSystem, 
        AttackConfig attackConfig, SlimeConfig slimeConfig, StateController stateController, Transform playerTransform)
    {
        InitializeBaseComponents(damageSystem, stateController);
        
        if (damageSystem != null && slimeConfig != null && attackConfig != null)
        {
            _slimeConfig = slimeConfig;
            CurrentConfig = attackConfig;
        }

        if (CurrentConfig.nameAttack == attackName)
        {
            var sortedQueue = CurrentConfig.animAttackSettings.OrderBy(a => a.countInQueue).ToList();
        
            foreach (var attack in sortedQueue)
            {
                _attackQueue.TryAdd(attack.countInQueue, attack);
            }

            MaxComboAttack = _attackQueue.Count;
            
            PlayerTransform = playerTransform;
        }
    }

    protected override void Update()
    {
        if (Time.time - lastAttackTime > comboWindow)
        {
            Exit();
        }
        
        if (CooldownAttack > 0)
        {
            CooldownAttack -= Time.deltaTime;
        }
    }

    public override void TryAttack()
    {
        if (_attackQueue.TryGetValue(CurrentCountAttack, out var value) )
        {
            if (CanAttack())
            {
                Debug.Log("Attack");
                StartAttack(value);
            }
            else if (!CheckAttackDistance(CurrentConfig.attackDistance))
            {
                animator.ResetTrigger(value.nameTrigger);
                ExitCorutine = StartCoroutine(ExitFromCombo());
            }
        }
        
        Exit();
    }

    private void Exit()
    {
        if (CurrentCountAttack >= MaxComboAttack)
        {
            if (ExitCorutine == null)
            {
                Debug.Log("Reset");
                ExitCorutine = StartCoroutine(ExitFromCombo());
            }
        }
    }

    private void StartAttack(AnimAttackSettings value)
    {
        StateController.ChangeStateAttack(true);
        DamageSystem?.DamageUpdate(CurrentConfig);
                
        lastAttackTime = Time.time;
                
        Attack(value);
                
        CurrentCountAttack++;
    }
    
    private IEnumerator ExitFromCombo()
    {
        ResetCooldownAttack(CurrentConfig.cooldownAttack);
        
        yield return new WaitForSeconds(exitDelay);
        
        foreach (var currentAttack in _attackQueue)
        {
            animator.ResetTrigger(currentAttack.Value.nameTrigger);
        }
        
        ExitAction();
    }

    private bool CanAttack()
    {
        return CooldownAttack <= 0 && CheckAttackDistance(CurrentConfig.attackDistance) &&
               StateController.CanAttack() && CurrentCountAttack < MaxComboAttack;
    }
}
