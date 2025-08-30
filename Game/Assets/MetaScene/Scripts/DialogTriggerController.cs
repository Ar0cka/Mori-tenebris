using System;
using System.Collections;
using System.Collections.Generic;
using Actors.NPC.DialogSystem;
using Actors.NPC.DialogSystem.TestUI;
using Actors.NPC.Inventory;
using ConsoleApp.Runtime;
using DG.Tweening;
using PlayerContextProviders;
using UnityEngine;

public class DialogTriggerController : MonoBehaviour
{
    [SerializeField] private TestDialogUI testDialogUI;

    private bool _playerInTrigger;
    private Sequence _messageSequence;
    
    private GameObject _player;
    private DialogFSM _dialogFsm;

    public void Initialize(DialogFSM fsm)
    {
        if (fsm == null)
            ConsoleLogger.Info("fsm is null");
        
        _dialogFsm = fsm;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Отображение кнопки взаимодействия
            _playerInTrigger = true;
            _player = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
            _player = null;
        }
    }

    private void Update()
    {
        if (!_playerInTrigger) return;
        
        InputLogic();
    }

    private void InputLogic()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var dialogProvider = _player.GetComponentInChildren<IPlayerContextProviders<PlayerDialogContext>>();

            if (dialogProvider == null || _dialogFsm == null)
            {
                ConsoleLogger.Error("Not find components");
                return;
            }
            
            _dialogFsm.UpdatePlayerDialogContext(dialogProvider.GetPlayerContext());
            testDialogUI.StartDialog();
        }
    }

    private void InteractiveMessage()
    {
        if (!_playerInTrigger) return;
        
        _messageSequence = DOTween.Sequence();
        
        //Логика интеравктивной кнопки
    }

    private void OnDestroy()
    {
        _messageSequence?.Kill();
    }
}
