using System;
using System.Collections;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.TestUI;
using DG.Tweening;
using UnityEngine;

public class TriggerControllerForNpc : MonoBehaviour
{
    [SerializeField] private TestDialogUI testDialogUI;

    private bool _playerInTrigger;
    private Sequence _messageSequence;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Отображение кнопки взаимодействия
            _playerInTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
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
