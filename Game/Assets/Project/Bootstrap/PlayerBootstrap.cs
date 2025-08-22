using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using NegativeEffects;
using Player.Inventory;
using PlayerNameSpace;
using UI.EffectUI;
using UI.Player.Log;
using UI.PlayerHpBar;
using UnityEngine;
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
    
    [Header("Inventory")]
    [SerializeField] private InventoryPanel inventoryLogic;
    
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

        // Serialized fields
        Check(nameof(playerUIManager), playerUIManager, out _valid);
        Check(nameof(passiveRegenerationStats), passiveRegenerationStats, out _valid);
        Check(nameof(stateMachineRealize), stateMachineRealize, out _valid);
        Check(nameof(playerLogController), playerLogController, out _valid);
        Check(nameof(effectUIController), effectUIController, out _valid);
        Check(nameof(playerHealthBar), playerHealthBar, out _valid);
        Check(nameof(playerGetStats), playerGetStats, out _valid);
        Check(nameof(hitLog), hitLog, out _valid);

        // Injected fields
        Check(nameof(_playerData), _playerData, out _valid);
        Check(nameof(_health), _health, out _valid);
        Check(nameof(_armour), _armour, out _valid);
        Check(nameof(_stamina), _stamina, out _valid);
        Check(nameof(_damageSystem), _damageSystem, out _valid);

        return _valid;
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
    }

    private void InitializeInventory()
    {
        inventoryLogic.Initialize();
    }
    
    private void Check(string objName, object obj, out bool valid)
    {
        if (obj == null)
        {
            Debug.LogError($"[PlayerBootstrap] Missing reference: {objName}");
            valid = false;
        }

        valid = true;
    }
}
