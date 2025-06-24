using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventBusNamespace;
using Systems.SpawnMonsterSystem;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class LogController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private SpawnObject spawnObject;
    [SerializeField] private Transform startPoint;
    
    [Header("Pool settings")]
    [SerializeField] private PrefabSpawnSettings spawnSettings;
    [SerializeField] private int startPoolCapacity;
    
    [Header("Settings")] 
    [SerializeField] private float endValues;
    [SerializeField] private float duration;
    [SerializeField] private int maxCount;
    
    private Action<LogText> _showLog;
    
    private string _currentLog = "";
    
    private ObjectPool _pool;
    private int _activeMessageCount;
    
    public void Initialize()
    {
        _showLog = text => ShowText(text.MessageText, text.LogType);
        EventBus.Subscribe(_showLog);

        _pool = new ObjectPool(spawnSettings, spawnObject, startPoolCapacity);
    }

    private void ShowText(string inputText, CustomLogType logType = CustomLogType.Message)
    {
        if (_activeMessageCount >= maxCount || _currentLog == inputText) return;
        
        GameObject item = _pool.Get();

        if (item == null)
        {
            Debug.LogError("Can't get item from pool");
            return;
        }

        _activeMessageCount++;
        
        item.transform.position = startPoint.position;
        
        TextMeshProUGUI textComponent = item.GetComponent<TextMeshProUGUI>();
        
        textComponent.text = inputText;
        textComponent.color = GetColor(logType);
        _currentLog = inputText;
        
        item.SetActive(true);
        
        Vector2 targetPos = (Vector2)item.transform.position + new Vector2(0,endValues);

        var tween = item.transform.DOMove(targetPos, duration).SetEase(Ease.Linear);
        
        StartCoroutine(HideText(item, tween));
    }

    private IEnumerator HideText(GameObject item, Tween tween)
    {
        yield return new WaitForSeconds(duration);
        _pool.ReturnItemToPool(item);
        tween.Kill();
        _currentLog = "";
        _activeMessageCount--;
        
    }

    private Color GetColor(CustomLogType type)
    {
        switch (type)
        {
            case CustomLogType.Message:
                return Color.white;
            case CustomLogType.Error:
                return Color.red;
            default:
                return Color.yellow;
        }
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe(_showLog);
    }
}

public class LogText
{ 
    public string MessageText { get; private set;}
    public CustomLogType LogType { get; private set; }

    public LogText(string messageText, CustomLogType logType)
    {
        MessageText = messageText;
        LogType = logType;
    }
}
