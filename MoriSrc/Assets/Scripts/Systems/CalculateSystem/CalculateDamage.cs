using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Systems.CalculateDamageSystem
{
    public static class CalculateDamage
    {
        public static int CalculateFinalDamageWithResist (int damage, float armour)
        {
            float multiplyDamage = damage / (damage + armour);
            return Mathf.FloorToInt(damage * multiplyDamage);
        }
    }
}