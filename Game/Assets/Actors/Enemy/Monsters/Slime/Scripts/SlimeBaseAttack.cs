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
    [SerializeField] private string attackName;
    [SerializeField] private float exitDelay;
    [SerializeField] private float comboWindow;

    public string AttackName => attackName;

    private AttackConfig _attackConfig;
    private SlimeConfig _slimeConfig;
    
    private Dictionary<int, AnimAttackSettings> _attackQueue = new Dictionary<int, AnimAttackSettings>();
    
    private float lastAttackTime;
    
    public void InitializeAttack(EnemyDamage damageSystem, 
        AttackConfig attackConfig, SlimeConfig slimeConfig, StateController stateController)
    {
        InitializeBaseComponents();

        Debug.Log("attackConfig type = " + attackConfig);
        
        if (damageSystem != null && slimeConfig != null && attackConfig != null)
        {
            DamageSystem = damageSystem;
            _slimeConfig = slimeConfig;
            _attackConfig = attackConfig;
        }

        if (_attackConfig.nameAttack == attackName)
        {
            var sortedQueue = _attackConfig.animAttackSettings.OrderBy(a => a.countInQueue).ToList();
        
            foreach (var attack in sortedQueue)
            {
                _attackQueue.TryAdd(attack.countInQueue, attack);
            }

            MaxComboAttack = _attackQueue.Count;
        }

        StateController = stateController;
    }

    private void Update()
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
                StartAttack(value);
            }
            else if (!CheckAttackDistance(_attackConfig.attackDistance))
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
        DamageSystem?.DamageUpdate(_attackConfig);
                
        lastAttackTime = Time.time;
                
        CurrentConfig = _attackConfig;
                
        Attack(value);
                
        CurrentCountAttack++;
    }
    
    private IEnumerator ExitFromCombo()
    {
        yield return new WaitForSeconds(exitDelay);
        
        foreach (var currentAttack in _attackQueue)
        {
            animator.ResetTrigger(currentAttack.Value.nameTrigger);
        }
        
        ExitAction();
    }

    private bool CanAttack()
    {
        return CooldownAttack <= 0 && CheckAttackDistance(_attackConfig.attackDistance) &&
               StateController.CanAttack() && CurrentCountAttack < MaxComboAttack;
    }
}
