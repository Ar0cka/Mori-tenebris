using System;
using System.Collections.Generic;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using EventBusNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI strength;
        [SerializeField] private TextMeshProUGUI agility;
        [SerializeField] private TextMeshProUGUI vitality;
        [SerializeField] private TextMeshProUGUI lucky;
        [SerializeField] private TextMeshProUGUI physicArmour;
        [SerializeField] private TextMeshProUGUI magicArmour;
        [Header("Level UI")] 
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI experienceText;
        [SerializeField] private TextMeshProUGUI upgradeTokensText;

        [Inject] private IGetPlayerStat _getPlayerStat;
        [Inject] private Armour _armour;
        
        private PlayerDataStats _playerData;

        private void OnEnable()
        {
            EventBus.Subscribe<SendUIUpdateExpSystem>(e => UpdateLevelUI());
            EventBus.Subscribe<SendUpdateStatEvent>(e => UpdateStatsUI());
        }

        public void Initialize()
        {
            _playerData = _getPlayerStat.GetPlayerDataStats();
            
            UpdateLevelUI();
            UpdateStatsUI();
        }

        private void UpdateStatsUI()
        {
            strength.text = $"Strength = {_playerData.Strength}";
            agility.text = $"Agility = {_playerData.Agility}";
            vitality.text = $"Vitality = {_playerData.Vitality}";
            lucky.text = $"Lucky = {_playerData.Lucky}";

            physicArmour.text = $"Physic armour = {_armour.PhysicArmour.ToString()}";
            magicArmour.text = $"Magic armour = {_armour.MagicArmour.ToString()}";
        }
        
        private void UpdateLevelUI()
        {
            levelText.text = $"Level = {_playerData.Level}";
            experienceText.text = $"Exp = {_playerData.CurrentExperience}/{_playerData.ExperienceForNextLevel}";
            upgradeTokensText.text = $"Upgrade tokens = {_playerData.UpgradeTokenCount}";
        }
        
        public void OnDestroy()
        {
            EventBus.Unsubscribe<SendUIUpdateExpSystem>(e => UpdateLevelUI());
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => UpdateStatsUI());
        }
    }
}