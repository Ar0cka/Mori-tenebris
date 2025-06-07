using System;
using Actors.Enemy.Stats.Scripts;
using UnityEngine;

namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public class ChangeInterrupt : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        
        private StateController _stateController;

        public void Initialize()
        {
            _stateController = enemyData.GetStateController();
        }

        public void EnableInterrupt()
        {
            _stateController.ChangeInterruptAttack(true);
        }

        public void DisableInterrupt()
        {
            _stateController.ChangeInterruptAttack(false);
        }
    }
}