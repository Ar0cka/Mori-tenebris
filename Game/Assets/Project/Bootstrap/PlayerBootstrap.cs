using System;
using System.Collections;
using System.Collections.Generic;
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
        SpawnPlayer();
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
        
        #region UI
            
        playerUIManager.Initialize();
        playerLogController.Initialize();
        effectUIController.Init();

        #endregion
    }
}
