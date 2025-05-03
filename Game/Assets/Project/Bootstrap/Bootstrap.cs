using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using Player.Inventory;
using PlayerNameSpace;
using PlayerNameSpace.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerStatsUI playerUIManager;
        [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
        [FormerlySerializedAs("playerInterface")] [SerializeField] private PlayerUI player;
        [SerializeField] private StateMachineRealize stateMachineRealize;
        
        
        [Inject] private PlayerData _playerData;
        [Inject] private InventoryLogic _inventoryLogic;
        [Inject] private Health _health;
        [Inject] private Armour _armour;
        [Inject] private Stamina _stamina;
        
        [Header("Inventory")]
        [SerializeField] private Transform slotContent;
        
        private void Awake()
        {
            _playerData.Initialize();
            
            #region Stats

            _armour.Initialize();
            _health.Initialize();
            _stamina.Initialize();

            #endregion
            
            passiveRegenerationStats.Initialize();
            stateMachineRealize.Initialize(_stamina);
            
            _inventoryLogic.Initialize(slotContent);
            
            #region UI

            player.Initialize();
            playerUIManager.Initialize();

            #endregion
        }
    }
}