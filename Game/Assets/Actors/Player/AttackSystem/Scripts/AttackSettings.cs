using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actors.Player.AttackSystem.Scripts
{
    [Serializable]
    public class AttackSettings
    {
        public float comboWindow;
        public float delayBetweenAttacks;
        public float attackCooldown;
    }

    [Serializable]
    public class AttackData
    {
        public string triggerName;
        public int countQueue;
        public WeaponHitColliderSettings weaponHitColliderSettings;
    }
}