using System.IO;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using EventBusNamespace;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerData : IGetPlayerStat
    {
        private const string SCR_OBJ_PATH = "PlayerData";

        private PlayerScrObj _playerScrObj;
        private ISaveAndLoad _saveSystem;
        private PlayerDataStats _playerDataStats;
        private PlayerStaticData _playerDataDontChangableStats;

        private string _filePath;

        public PlayerData(ISaveAndLoad saveAndLoad)
        {
            _saveSystem = saveAndLoad;
            _playerScrObj = Resources.Load<PlayerScrObj>(SCR_OBJ_PATH);

            if (_playerScrObj == null)
                throw new FileNotFoundException($"PlayerScrObj not found at: {SCR_OBJ_PATH}");
        }

        public void Initialize()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            _playerDataDontChangableStats = _playerScrObj.StaticPlayerStats;

            EventBus.Subscribe<SendSavePlayerDataEvent>(e => SavePlayerData());
            
            _playerDataStats = _playerScrObj.PlayerDataStats.Clone();
            Debug.Log("Clone");
        }

        private PlayerDataStats LoadData()
        {
            PlayerDataStats loadData = _saveSystem.LoadData<PlayerDataStats>(_filePath);

            if (loadData != null)
            {
                return loadData;
            }

            return null;
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