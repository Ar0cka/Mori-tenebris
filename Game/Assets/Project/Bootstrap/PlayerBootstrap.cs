using System.Reflection;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using NegativeEffects;
using Player.Inventory;
using PlayerContextProviders;
using PlayerNameSpace;
using UI.EffectUI;
using UI.Player.Log;
using UI.PlayerHpBar;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerBootstrap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerStatsUI playerUIManager;
    [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
    [SerializeField] private StateMachineRealize stateMachineRealize;
    [SerializeField] private LogController playerLogController;
    [SerializeField] private EffectUIController effectUIController;
    [SerializeField] private PlayerHealthBar playerHealthBar;
    [SerializeField] private PlayerGeterStats playerGetStats;
    [SerializeField] private HitLog hitLog;
    
    [FormerlySerializedAs("inventoryLogic")]
    [Header("Inventory")]
    [SerializeField] private InventoryPanel inventoryPanel;
    
    [Header("Providers")]
    [SerializeField] private PlayerDialogContextProvider playerDialogContextProvider;
    
    [Inject] private DiContainer _diContainer;
    [Inject] private PlayerData _playerData;
    [Inject] private Health _health;
    [Inject] private Armour _armour;
    [Inject] private Stamina _stamina;
    [Inject] private DamageSystem _damageSystem;

    private bool _valid;

    private void Awake()
    {
        if (!ValidateSerializedFields())
        {
            Debug.LogError("PlayerBootstrap: Инициализация прервана из-за отсутствующих компонентов.");
            return;
        }

        SpawnPlayer();
    }
    
    private bool ValidateSerializedFields()
    {
        bool valid = true;
        var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var value = field.GetValue(this);
            if (value == null)
            {
                Debug.LogError($"[PlayerBootstrap] Missing reference: {field.Name}");
                valid = false;
            }
        }

        return valid;
    }

    private void SpawnPlayer()
    {
        playerHealthBar.Init();

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
        
        InitializeInventory();
        
        #region UI

        playerUIManager.Initialize();
        playerLogController.Initialize();
        effectUIController.Init();

        #endregion
        
        ProvidersInitialization();
    }

    private void InitializeInventory()
    {
        inventoryPanel.Initialize();
    }

    private void ProvidersInitialization()
    {
        playerDialogContextProvider.Initialize(inventoryPanel);
    }
}
