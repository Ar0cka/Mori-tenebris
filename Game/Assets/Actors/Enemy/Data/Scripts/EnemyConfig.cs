using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Actors.Enemy.Data.Scripts
{
    [Serializable]
    public class EnemyConfig
    {
        [Header("Descriptions")]
        public string enemyName;
        public string description;
        
        [Header("MoveSettings")]
        public float agressionDistance;
        public float speed;

        [Header("Data")]
        public int hitPoints;
        public float physicArmour;
        public float magicArmour;
        
        [Header("HitSettings")]
        public int maxCountHit;
        public float cooldownHit;

        [Header("Prefab")]
        public GameObject prefab;
    }
}