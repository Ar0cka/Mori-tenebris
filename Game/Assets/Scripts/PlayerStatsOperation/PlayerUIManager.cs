using System;
using System.Collections.Generic;
using EventBusNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> playerStatsUI;
        [Header("Level UI")] 
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI experienceText;
        [SerializeField] private TextMeshProUGUI upgradeTokensText;
        
        [Header("Stats upgrade buttons")]
        [SerializeField] private List<Button> buttons;

        [Inject] private IGetPlayerStat _getPlayerStat;
        
        private PlayerDataStats _playerData;
        
        public void Initialize()
        {
            EventBus.Subscribe<SendUIUpdateExpSystem>(UpdateLevelUI);
            
        }

        private void UpdateLevelUI(SendUIUpdateExpSystem e)
        {
            _playerData = _getPlayerStat.GetPlayerDataStats();

            levelText.text = $"Level = {_playerData.Level}";
            experienceText.text = $"Exp = {_playerData.CurrentExperience}/{_playerData.ExperienceForNextLevel}";
            upgradeTokensText.text = $"Upgrade tokens = {_playerData.UpgradeTokenCount}";

            if (!_playerData.CheckInMaxLevel())
            {
                EventBus.Unsubscribe<SendUIUpdateExpSystem>(UpdateLevelUI);
                InteractableButtons();
            }
        }

        private void InteractableButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = _playerData.UpgradeTokenCount == 0;
            }
        }
        
        public void OnDestroy()
        {
            EventBus.Unsubscribe<SendUIUpdateExpSystem>(UpdateLevelUI);
        }
    }
}