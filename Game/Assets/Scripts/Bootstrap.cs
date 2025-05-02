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
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerStatsUI playerUIManager;
        [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
        [SerializeField] private InterfacePlayerManager playerInterface;
        [SerializeField] private StateMachineRealize stateMachineRealize;
        [SerializeField] private Inventory inventory;
        
        [Inject] private Health _health;
        [Inject] private Armour _armour;
        [Inject] private Stamina _stamina;
        
        private void Awake()
        {
            playerData.Initialize();
            
            #region Stats

            _armour.Initialize();
            _health.Initialize();
            _stamina.Initialize();

            #endregion
            
            passiveRegenerationStats.Initialize();
            stateMachineRealize.Initialize(_stamina);
            
            inventory.Initialize();
            
            #region UI

            playerInterface.Initialize();
            playerUIManager.Initialize();

            #endregion
        }
    }
}