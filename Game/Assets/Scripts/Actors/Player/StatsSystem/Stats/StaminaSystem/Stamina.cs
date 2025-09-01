using System;
using EventBusNamespace;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class Stamina : ISubtractionStamina, IRegenerationStamina, IDisposable
    {
        [Inject] private IGetPlayerStat _playerStat;
        
        private int maxStamina;
        public int CurrentStamina { get; private set; }
        
        private PlayerDataStats _playerDataStats => _playerStat.GetPlayerDataStats();

        public void Initialize()
        {
            EventBus.Subscribe<SendUpdateStatEvent>(e => UpdateStamina());
            UpdateStamina();
        }

        public void SubtractionStamina(int value)
        {
            CurrentStamina -= Math.Clamp(value, 0, CurrentStamina);
        }

        public void RegenerationStamina(int value)
        {
            CurrentStamina += value;
        }
        
        private void UpdateStamina()
        {
            maxStamina = _playerStat.GetPlayerDataStaticStats().StartMaxStamina + _playerDataStats.Strength * 5;
            CurrentStamina = maxStamina;
        }

        public int GetMaxStamina()
        {
            return maxStamina;
        }
        
        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => UpdateStamina());
        }
    }
}