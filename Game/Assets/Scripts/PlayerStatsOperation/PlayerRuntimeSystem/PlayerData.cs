using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.PlayerStatsOperation
{
    public class PlayerData : MonoBehaviour, IGetPlayerStat
    {
        [SerializeField] private PlayerScrObj _playerScrObj;

        private List<PlayerDataStats> playerDataStats = new List<PlayerDataStats>();

        private Dictionary<string, PlayerDataStats> PlayerDataStatsList = new Dictionary<string, PlayerDataStats>();
        
        private PlayerDataDontChangableStats playerDataDontChangableStats;
        
        public PlayerDataDontChangableStats GetPlayerDataStaticStats() => playerDataDontChangableStats;

        private string filePath;
        
        public void Initialize()
        {
            filePath = Path.Combine(Application.persistentDataPath, "playerStats.json");
            
            playerDataDontChangableStats = _playerScrObj.StaticPlayerStats;
            PlayerDataStatsList.Clear();

            var statList = LoadStat();

            if (statList != null && statList.Count != 0)
            {
                playerDataStats = statList;
            }
            else
            {
                foreach (var playerData in _playerScrObj.PlayerDataStats)
                {
                    playerDataStats.Add(playerData.Clone());
                }
            }

            foreach (var stat in playerDataStats)
            {
                if (!PlayerDataStatsList.ContainsKey(stat.nameState))
                {
                    PlayerDataStatsList.Add(stat.nameState, stat);
                }
                else
                {
                    Debug.LogError("Duplicate stat");
                }
               
            }
        }

        public PlayerDataStats TryGetStat(string name)
        {
            if (PlayerDataStatsList.TryGetValue(name, out var stat))
            {
                return stat;
            }

            return null;
        }

        #region SaveRuntimeStatsSystem

        public void SaveStat()
        {
            try
            {
                var listInSave = new ContainerInList<PlayerDataStats> { dataListInSave = playerDataStats };
                string data = JsonUtility.ToJson(listInSave, true);
                File.WriteAllText(filePath, data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private List<PlayerDataStats> LoadStat()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogError("Not fiend path");
                    return null;
                }
                
                string dataReadText = File.ReadAllText(filePath);
                var loadDataContainer = JsonUtility.FromJson<ContainerInList<PlayerDataStats>>(dataReadText);

                return loadDataContainer?.dataListInSave;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private void OnApplicationQuit()
        {
            SaveStat();
        }

        #endregion
    }

    [Serializable]
    public class ContainerInList<T>
    {
        public List<T> dataListInSave;
    }
}