using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Actors.Enemy.Data.Scripts
{
    [Serializable]
    public class EnemyConfig
    {
        public string enemyName;
        public string description;
        
        public float agressionDistance;

        public float speed;

        public int hitPoints;

        public float physicArmour;
        public float magicArmour;

        public GameObject prefab;
    }
}