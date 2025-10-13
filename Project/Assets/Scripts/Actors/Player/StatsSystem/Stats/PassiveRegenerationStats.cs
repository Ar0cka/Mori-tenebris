using System;
using Actors.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace PlayerNameSpace
{
    public class PassiveRegenerationStats : MonoBehaviour
    {
        [SerializeField] private int checkThresholdHealth = 1;
        [SerializeField] private int checkThresholdStamina = 1;

        [SerializeField] private PlayerController playerController;

        private float _healingCount;
        private float _staminaCount;

        private PlayerDataStats _playerData;
        private IRegenerationHealth _regenerationHealth;
        private IRegenerationStamina _regenerationStamina;

        private bool _initializeCompleted;

        public void Initialize()
        {
            _playerData = playerController.GetPlayerStat().GetPlayerDataStats();
            _regenerationHealth = playerController.RegenerationHealth();
            _regenerationStamina = playerController.RegenerationStamina();
            
            _initializeCompleted = true;
        }

        public void FixedUpdate()
        {
            if (_initializeCompleted)
            {
                if (CanRegenerateHealth())
                {
                    Healing();
                }

                if (CanRegenerateStamina())
                {
                    RegenerationStamina();
                }
            }
        }

        private void Healing()
        {
            _healingCount += _playerData.SpeedRegenerationHealth;

            if (_healingCount >= checkThresholdHealth)
            {
                int addHealth = Mathf.FloorToInt(_healingCount);
                _regenerationHealth.Regeneration(addHealth);
                _healingCount -= addHealth;
            }
        }

        private void RegenerationStamina()
        {
            _staminaCount += _playerData.SpeedRegenerationStamina;
            
            if (_staminaCount >= checkThresholdStamina)
            {
                int addStamina = Mathf.FloorToInt(_staminaCount);
                _regenerationStamina.RegenerationStamina(addStamina);
                _staminaCount -= addStamina;
            }
        }

        private bool CanRegenerateStamina()
        {
            return _regenerationStamina.CurrentStamina < _regenerationStamina.GetMaxStamina();
        }

        private bool CanRegenerateHealth()
        {
            return _regenerationHealth.CurrentHitPoint < _regenerationHealth.GetMaxHealth();
        }
    }
}