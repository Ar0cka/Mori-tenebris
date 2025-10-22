using System;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Movement.Base;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Bootstrap
{
    public class MonsterBootstrap : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private ChangeInterrupt interrupt;
        [SerializeField] private MonstersBattleController monstersBattleController;
        [SerializeField] private EnemyMoveFsmRealize realize;

        private void Awake()
        {
            enemyData.Initialize();
            realize.Initialize();
            interrupt.Initialize();
            
            if (monstersBattleController != null) 
                monstersBattleController.Initialize();
        }
    }
}