using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.AttackSystem.Scripts;
using UnityEngine;

public class EnemyAttack : AttackEnemyAbstract
{
    [SerializeField] private List<string> animationAttack;
    
    public override void AssingAttack()
    {
        base.AssingAttack();
        Attack(animationAttack);
    }
}
