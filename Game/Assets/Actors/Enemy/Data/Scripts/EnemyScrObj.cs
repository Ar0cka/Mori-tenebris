using System;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Actors.Enemy.EnemyData.Scripts
{
    [CreateAssetMenu(fileName = "enemy", menuName = "Create/enemy", order = 0)]
    public class EnemyScrObj : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private string enemyName;
        [SerializeField] private string description;
        
        [Header("Stats")]
        [SerializeField] private int hitPoints;
        [SerializeField] private float armour;
        
        [Header("Damage")]
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;

        [Header("Armour")] 
        [SerializeField] private float physicArmour;
        [SerializeField] private float magicArmour;
        
        [Header("Prefab")]
        [SerializeField] private GameObject prefab;

        public string EnemyName => enemyName;
        public string Description => description;
        
        public int HitPoints => hitPoints;
        public float Armour => armour;
        
        public int Damage => damage;
        public DamageType DamageType => damageType;
        
        public float PhysicArmour => physicArmour;
        public float MagicArmour => magicArmour;
        
        public GameObject Prefab => prefab;
    }
}