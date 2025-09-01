using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.Serialization;

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
        [Header("Anim settings")]
        public string nameTrigger;
        public int countInQueue;
        
        [Header("Delays")] 
        public float startAttackDelay;
        public float hitDelay;
    }
}