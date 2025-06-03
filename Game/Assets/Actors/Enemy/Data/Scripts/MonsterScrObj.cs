using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Data.Scripts
{
    public abstract class MonsterScrObj: ScriptableObject
    {
        public abstract EnemyConfig GetConfig();

        public abstract List<AttackConfig> GetAttackConfig();
    }
}