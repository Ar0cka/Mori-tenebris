using System.Collections.Generic;
using Project.Bootstrap;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using EventBusNamespace;
using NegativeEffects;
using PlayerNameSpace;
using PlayerNameSpace.Inventory;
using Systems.SpawnMonsterSystem;
using UI;
using UI.EffectUI;
using UI.Player.Log;
using UI.PlayerHpBar;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Spawn")] 
        [SerializeField] private SpawnPlayer spawnPlayer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private SpawnMonster spawnMonster;
        
        private void Awake()
        {
           Initialize();
           EventBus.Publish(new GameInitialized());
        }

        private void Initialize()
        {
            spawnPlayer.SetStartPosition(spawnPoint);
        }
        
        private void SpawnMonster()
        {
            spawnMonster.Initialize();
        }
    }

    public class GameInitialized
    {
        
    }
}