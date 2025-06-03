using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.AttackSystem.Scripts;
using Actors.Enemy.Data.Scripts;
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

    private Coroutine _exitCorutine;
    private float lastAttackTime;
    
    public bool isAttacking { get; private set; }
    
    public void InitializeAttack(EnemyDamage damageSystem, 
        AttackConfig attackConfig, SlimeConfig slimeConfig)
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
    }

    public override void AssingBaseAttack()
    {
        if (CooldownAttack > 0)
        {
            CooldownAttack -= Time.deltaTime;
            return;
        }
        
        if (_attackQueue.TryGetValue(CurrentCountAttack, out var value) && CurrentCountAttack < MaxComboAttack)
        {
            if (CooldownAttack <= 0 && CheckAttackDistance(_attackConfig.attackDistance) && _exitCorutine == null)
            {
                isAttacking = true;
                DamageSystem?.DamageUpdate(_attackConfig);
                CurrentConfig = _attackConfig;
                Attack(value.nameTrigger);

                if (AnimationPlayChecker())
                {
                    CurrentCountAttack++;
                }
            }
            else if (!CheckAttackDistance(_attackConfig.attackDistance))
            {
                animator.ResetTrigger(value.nameTrigger);
                _exitCorutine = StartCoroutine(ExitFromCombo());
            }
        }
        else if (CurrentCountAttack >= MaxComboAttack)
        {
            if (_exitCorutine == null)
            {
                _exitCorutine = StartCoroutine(ExitFromCombo());
            }
        }
    }
    
    public override void Attack(string attackAnimation)
    {
        base.Attack(attackAnimation);
    }

    private IEnumerator ExitFromCombo()
    {
        ResetCooldownAttack(CurrentConfig.cooldownAttack);

        yield return new WaitForSeconds(exitDelay);

        foreach (var currentAttack in _attackQueue)
        {
            animator.ResetTrigger(currentAttack.Value.nameTrigger);
        }

        isAttacking = false;
        CurrentCountAttack = 0;
        _exitCorutine = null;
    }
}
