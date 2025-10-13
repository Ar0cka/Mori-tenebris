using System;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Movement;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;

namespace Project.Bootstrap
{
    public class MonsterBootstrap : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private ChangeInterrupt interrupt;
        [SerializeField] private MonstersBattleController monstersBattleController;
        [SerializeField] private EnemyMove enemyMove;

        private void Awake()
        {
            enemyData.Initialize();
            enemyMove.Initialize();
            interrupt.Initialize();
            
            if (monstersBattleController != null) 
                monstersBattleController.Initialize();
        }
    }
}