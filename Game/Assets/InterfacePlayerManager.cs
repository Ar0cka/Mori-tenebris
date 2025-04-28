using System;
using System.Collections;
using System.Collections.Generic;
using PlayerNameSpace;
using TMPro;
using UnityEngine;
using Zenject;

public class InterfacePlayerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentStamina;
    [SerializeField] private TextMeshProUGUI currentHealth;
    
    [Inject] private IRegenerationStamina _regenerationStamina;
    [Inject] private IRegenerationHealth _regenerationHealth;
    
    private bool _isInitialized = false;

    public void Initialize()
    {
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized)
        {
            currentHealth.text = "Health: " + _regenerationHealth.CurrentHitPoint;
            currentStamina.text = "Stamina: " + _regenerationStamina.CurrentStamina;
        }
    }
}
