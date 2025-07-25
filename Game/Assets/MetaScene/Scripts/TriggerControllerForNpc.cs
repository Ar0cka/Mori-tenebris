using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriggerControllerForNpc : MonoBehaviour
{
    //Ссылка на контроллер диалога

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
