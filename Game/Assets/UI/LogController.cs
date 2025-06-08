using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventBusNamespace;
using TMPro;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField] private Transform textTransform;
    [SerializeField] private TextMeshProUGUI logText;
    
    private Action<LogText> _showLog;
    private Sequence _mainASequence;
    
    private Vector2 _startPos;
    
    private void Start()
    {
        _showLog = text => ShowText(text.MessageText);
        EventBus.Subscribe(_showLog);
        _startPos = textTransform.position;
    }

    private void ShowText(string logText)
    {
        _mainASequence?.Kill();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(_showLog);
    }
}

public class LogText
{ 
    public string MessageText { get; private set;}

    public LogText(string messageText)
    {
        MessageText = messageText;
    }
}
