using Project.Bootstrap;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using EventBusNamespace;
using PlayerNameSpace;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerStatsUI playerUIManager;
        [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
        [SerializeField] private PlayerUI player;
        [SerializeField] private StateMachineRealize stateMachineRealize;
        
        [Header("Inventory")]
        [SerializeField] private Transform slotContent;
        [SerializeField] private InventoryScrObj inventoryConfig;

        [Header("Spawn")] 
        [SerializeField] private SpawnPlayer spawnPlayer;
        
        [Inject] private PlayerData _playerData;
        [Inject] private InventoryLogic _inventoryLogic;
        [Inject] private Health _health;
        [Inject] private Armour _armour;
        [Inject] private Stamina _stamina;

        
        private void Awake()
        {
           Initialize();
           EventBus.Publish(new GameInitialized());
        }

        private void Initialize()
        {
            SpawnPlayer();  
            
            #region UI

            player.Initialize();
            playerUIManager.Initialize();

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

            #endregion
            
            stateMachineRealize.Initialize(_stamina);
            passiveRegenerationStats.Initialize();
            
            
            _inventoryLogic.Initialize(slotContent, inventoryConfig);
        }
    }

    public class GameInitialized
    {
        
    }
}