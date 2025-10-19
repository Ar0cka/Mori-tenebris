using System.Reflection;
using Actors.Player;
using ConsoleApp.Runtime.ConsoleAttribute;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using EconomicSystem;
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
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStatsUI playerUIManager;
    [SerializeField] private PassiveRegenerationStats passiveRegenerationStats;
    [SerializeField] private StateMachineRealize stateMachineRealize;
    [SerializeField] private LogController playerLogController;
    [SerializeField] private EffectUIController effectUIController;
    [SerializeField] private PlayerHealthBar playerHealthBar;
    [SerializeField] private PlayerGeterStats playerGetStats;
    [SerializeField] private HitLog hitLog;
    [SerializeField] private PlayerTakeDamage playerTakeDamage;

    [Header("Economic")] 
    [SerializeField] private WalletRealize wallet;
    [Header("Inventory")]
    [SerializeField] private InventoryPanel inventoryPanel;
    
    [Header("Providers")]
    [SerializeField] private PlayerDialogContextProvider playerDialogContextProvider;
    
    [Inject] private DiContainer _diContainer;

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
        
        playerGetStats.Init();
        hitLog.Initialize();

        playerController.InitializePlayer();
        
        stateMachineRealize.Initialize(playerController.SubtractionStamina());
        passiveRegenerationStats.Initialize();
        
        InitializeInventory();
        
        #region UI

        playerUIManager.Initialize(playerController);
        playerLogController.Initialize();
        effectUIController.Init();

        #endregion
        
        playerTakeDamage.Initialize(playerController.HitPlayer());
        
        wallet.Initialize();
        
        ProvidersInitialization();
        
        CommandRegister.Register(typeof(PlayerBootstrap).Assembly);
    }

    private void InitializeInventory()
    {
        inventoryPanel.Initialize();
    }

    private void ProvidersInitialization()
    {
        playerDialogContextProvider.Initialize(inventoryPanel, wallet);
    }
}
