using System;
using Actors.Enemy.Data.Scripts;
using UnityEngine;

namespace Actors.Enemy.Monsters.Slime.Data.Scripts
{
    [Serializable]
    public class SlimeConfig : EnemyConfig
    {
        [Header("Jump settings")]
        public float jumpForceY;
        public float jumpForceX;
        public float jumpTime;
        
        public float damageOutJump;
        public float startShield;
    }
}