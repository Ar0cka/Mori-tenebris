using System.IO;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using EventBusNamespace;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerData : MonoBehaviour, IGetPlayerStat
    {
        [SerializeField] private PlayerScrObj playerScrObj;
        
        [Inject] private ISaveAndLoad _saveSystem;

        private PlayerDataStats _playerDataStats;

        private PlayerStaticData _playerDataDontChangableStats;
        
        private string _filePath;

        public void Initialize()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "playerData.json");

            EventBus.Subscribe<SendSavePlayerDataEvent>(e => SavePlayerData());
            
            _playerDataDontChangableStats = playerScrObj.StaticPlayerStats;

            PlayerDataStats loadData = _saveSystem.LoadData<PlayerDataStats>(_filePath);

            if (loadData != null)
            {
                _playerDataStats = loadData;
            }
            else
            {
                _playerDataStats = playerScrObj.PlayerDataStats.Clone();
            }
        }

        public void ResetData()
        {
            _playerDataStats = playerScrObj.PlayerDataStats.Clone();
        }

        #region SaveData

        private void SavePlayerData()
        {
            Debug.Log("SavePlayerData");
            _saveSystem.SaveData(_playerDataStats, _filePath);
        }
        
        private void OnApplicationQuit()
        {
            EventBus.Unsubscribe<SendSavePlayerDataEvent>(e => SavePlayerData());
        }

        #endregion
        
        #region Getters

        public PlayerStaticData GetPlayerDataStaticStats() => _playerDataDontChangableStats;

        public PlayerDataStats GetPlayerDataStats() => _playerDataStats;

        #endregion
    }
}