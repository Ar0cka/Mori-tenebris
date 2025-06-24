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
        [Header("Player")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerStatsUI playerUIManager;
        [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
        [SerializeField] private StateMachineRealize stateMachineRealize;
        [SerializeField] private LogController playerLogController;
        [SerializeField] private EffectUIController effectUIController;
        [SerializeField] private PlayerHealthBar playerHealthBar;
        [SerializeField] private PlayerGeterStats playerGetStats;
        [SerializeField] private HitLog hitLog;
        
        [Header("Inventory")]
        [SerializeField] private Transform slotContent;
        [SerializeField] private InventoryScrObj inventoryConfig;
        [SerializeField] private List<GameObject> equipSlots;

        [Header("Spawn")] 
        [SerializeField] private SpawnPlayer spawnPlayer;
        [SerializeField] private SpawnMonster spawnMonster;
        
        [Inject] private PlayerData _playerData;
        [Inject] private InventoryLogic _inventoryLogic;
        [Inject] private Health _health;
        [Inject] private Armour _armour;
        [Inject] private Stamina _stamina;
        [Inject] private DamageSystem _damageSystem;

        
        private void Awake()
        {
           Initialize();
           EventBus.Publish(new GameInitialized());
        }

        private void Initialize()
        {
            playerHealthBar.Init();
            
            SpawnPlayer();  
            //SpawnMonster();
            
            #region UI
            
            playerUIManager.Initialize();
            playerLogController.Initialize();
            effectUIController.Init();

            #endregion
        }

        private void SpawnPlayer()
        {
            spawnPlayer.SetStartPosition(playerTransform);
            
            _playerData.Initialize();
            
            #region Stats

            _armour.Initialize();
            _health.Initialize();
            _stamina.Initialize();
            _damageSystem.Initialize();
            playerGetStats.Init();
            hitLog.Initialize();
            
            #endregion
            
            stateMachineRealize.Initialize(_stamina);
            passiveRegenerationStats.Initialize();
            
            
            _inventoryLogic.Initialize(slotContent, inventoryConfig, equipSlots);
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