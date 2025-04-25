using System;
using UnityEngine;

namespace PlayerNameSpace
{
    [Serializable]
    public class PlayerStaticData
    {
        [SerializeField] private int startMaxHitPoint;
        [SerializeField] private int startMaxStamina;
        [SerializeField] private int walkSpeed;
        
        public int StartMaxHitPoint => startMaxHitPoint;
        public int StartMaxStamina => startMaxStamina;
        public int WalkSpeed => walkSpeed;
    }
}