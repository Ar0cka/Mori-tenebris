using System;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Actors.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Inject] private ISaveAndLoad _saveAndLoad;
        
        private PlayerData _playerData;
        private Health _health;
        private Armour _armour;
        private Stamina _stamina;
        private DamageSystem _damageSystem;
        
        public void InitializePlayer()
        {
            _playerData = new PlayerData(_saveAndLoad);
            _playerData.Initialize();
            
            _armour = new Armour(_playerData);
            _armour.Initialize();
            
            _health = new Health(_playerData, _armour);
            _health.Initialize();
            
            _stamina = new Stamina(_playerData);
            _stamina.Initialize();
            
            _damageSystem = new DamageSystem();
            _damageSystem.Initialize(_playerData);
        }
        
        public IGetPlayerStat GetPlayerStat() => _playerData;
        public IRegenerationHealth RegenerationHealth() => _health;
        public IHitPlayer HitPlayer() => _health;
        
        public Health GetHealth() => _health;
        
        public Stamina GetStamina() => _stamina;
        
        public Armour GetArmour() => _armour;
        public IDamageSystem DamageSystem() => _damageSystem;
        
        public IEquipAndUnEquipItem EquipAndUnEquipItem() => _armour;
        public IRegenerationStamina RegenerationStamina() => _stamina;
        public ISubtractionStamina SubtractionStamina() => _stamina;

        private void OnApplicationQuit()
        {
            _playerData.Dispose();
            _armour.Dispose();
            _stamina.Dispose();
            _health.Dispose();
            _damageSystem.Dispose();
        }
    }
}