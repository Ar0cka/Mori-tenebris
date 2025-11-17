using System;
using Actors.Enemy.Movement.Base;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Bootstrap
{
    public class MonsterBootstrap : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private EnemyMoveFsmRealize realize;
        private void Awake()
        {
            enemyData.Initialize();
            realize.Initialize();
        }
    }
}