using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerNameSpace
{
    [Serializable]
    public class PlayerStaticData
    {
        [SerializeField] private int baseDamage;
        [SerializeField] private int startMaxHitPoint;
        [SerializeField] private int startMaxStamina;
        [SerializeField] private int walkSpeed;
        [SerializeField] private int spritSpeed;
        [FormerlySerializedAs("staminaSubstractionInSecond")] [SerializeField] private float runSubtractionInSecond;

        [SerializeField] private float rollDistance;
        [SerializeField] private float rollDuration;
        [SerializeField] private float rollCooldown;
        [SerializeField] private int costRoll;
        
        public int BaseDamage => baseDamage;
        public int StartMaxHitPoint => startMaxHitPoint;
        public int StartMaxStamina => startMaxStamina;
        public int WalkSpeed => walkSpeed;
        public int SpritSpeed => spritSpeed;
        public float RunSubtraction => runSubtractionInSecond;
        public float RollDistance => rollDistance;
        public float RollCooldown => rollCooldown;
        public float RollDuration => rollDuration;
        public int CostRoll => costRoll;
    }
}