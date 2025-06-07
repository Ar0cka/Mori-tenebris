using System;
using Actors.Enemy.Data.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Monsters.Slime.Data.Scripts
{
    [Serializable]
    public class SlimeConfig : EnemyConfig
    {
        [Header("Jump settings")]
        public float jumpForce;
        public int jumpNums;
        public float jumpTime;
        public float landingBounceHeight;
        public float landingTime;
        
        public float damageOutJump;
        public float startShield;
        public float delayAfterJump;
    }
}