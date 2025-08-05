using System;
using System.Collections;
using System.Collections.Generic;
using Actors.NPC.DialogSystem;
using Actors.NPC.DialogSystem.TestUI;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using NegativeEffects;
using PlayerNameSpace;
using PlayerNameSpace.Inventory;
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
    [SerializeField] private DialogFsmRealize dialogFsm;
    [SerializeField] private TestDialogUI testDialogUI;
    
    [Header("Inventory")]
    [SerializeField] private Transform slotContent;
    [SerializeField] private InventoryScrObj inventoryConfig;
    [SerializeField] private List<GameObject> equipSlots;
    
    [Inject] private PlayerData _playerData;
    [Inject] private InventoryLogic _inventoryLogic;
    [Inject] private Health _health;
    [Inject] private Armour _armour;
    [Inject] private Stamina _stamina;
    [Inject] private DamageSystem _damageSystem;

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

        void Check(string name, object obj)
        {
            if (obj == null)
            {
                Debug.LogError($"[PlayerBootstrap] Missing reference: {name}");
                valid = false;
            }
        }

        // Serialized fields
        Check(nameof(playerUIManager), playerUIManager);
        Check(nameof(passiveRegenerationStats), passiveRegenerationStats);
        Check(nameof(stateMachineRealize), stateMachineRealize);
        Check(nameof(playerLogController), playerLogController);
        Check(nameof(effectUIController), effectUIController);
        Check(nameof(playerHealthBar), playerHealthBar);
        Check(nameof(playerGetStats), playerGetStats);
        Check(nameof(hitLog), hitLog);
        Check(nameof(dialogFsm), dialogFsm);
        Check(nameof(slotContent), slotContent);
        Check(nameof(inventoryConfig), inventoryConfig);
        Check(nameof(equipSlots), equipSlots);
        Check(nameof(testDialogUI), testDialogUI);

        // Injected fields
        Check(nameof(_playerData), _playerData);
        Check(nameof(_inventoryLogic), _inventoryLogic);
        Check(nameof(_health), _health);
        Check(nameof(_armour), _armour);
        Check(nameof(_stamina), _stamina);
        Check(nameof(_damageSystem), _damageSystem);

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
        
        _inventoryLogic.Initialize(slotContent, inventoryConfig, equipSlots);
        
        InitDialogSystem();
        
        #region UI
            
        playerUIManager.Initialize();
        playerLogController.Initialize();
        effectUIController.Init();

        #endregion
    }

    private void InitDialogSystem()
    {
        dialogFsm.Initialize();
        testDialogUI.Initialize(dialogFsm.GetDialogFsm());
        testDialogUI.ExitFromDialogMenu();
    }

    private void OnApplicationQuit()
    {
        dialogFsm.GetDialogFsm().Dispose();
    }
}
