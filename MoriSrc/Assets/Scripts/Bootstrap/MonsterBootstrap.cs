using System;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Movement;
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
        [FormerlySerializedAs("fsmRealize")] [SerializeField] private MileEnemyMoveRealize realize;

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