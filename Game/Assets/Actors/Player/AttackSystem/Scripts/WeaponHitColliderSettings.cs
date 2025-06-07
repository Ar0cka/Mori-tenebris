using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Player.AttackSystem.Scripts
{
    [Serializable]
    public class WeaponHitColliderSettings
    {
        public Vector2 hitSize;
        
        public float angleColliderRight;
        public float angleColliderLeft;
    }
}