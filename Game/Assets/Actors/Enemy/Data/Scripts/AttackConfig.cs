using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Actors.Enemy.Data.Scripts
{
    [Serializable]
    public class AttackConfig
    {
        [Header("Animations")]
        public string nameAttack;
        public List<AnimAttackSettings> animAttackSettings;
        
        [Header("Attack settings")]
        public DamageType damageType;
        public float cooldownAttack;
        public int damage;
        public float attackDistance;
    }

    [Serializable]
    public class AnimAttackSettings
    {
        public List<string> nameTriggers;
        public string nameTrigger;
        public int countInQueue;
    }
}