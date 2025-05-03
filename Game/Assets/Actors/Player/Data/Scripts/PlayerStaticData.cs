using System;
using UnityEngine;

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
        [SerializeField] private float staminaSubstractionInSecond;
        [SerializeField] private float speedRegenerationStamina;
        
        public int BaseDamage => baseDamage;
        public int StartMaxHitPoint => startMaxHitPoint;
        public int StartMaxStamina => startMaxStamina;
        public int WalkSpeed => walkSpeed;
        public int SpritSpeed => spritSpeed;
        public float StaminaSubstraction => staminaSubstractionInSecond;
    }
}