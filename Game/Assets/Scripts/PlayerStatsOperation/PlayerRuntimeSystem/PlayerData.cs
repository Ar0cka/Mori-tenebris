using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.PlayerStatsOperation
{
    public class PlayerData : MonoBehaviour, IGetPlayerStat
    {
        [SerializeField] private PlayerScrObj _playerScrObj;

        private PlayerDataStats playerDataStats;

        private PlayerDataDontChangableStats playerDataDontChangableStats;
        
        private string filePath;

        private event Action send;

        public void Initialize()
        {
            filePath = Path.Combine(Application.persistentDataPath, "playerStat.json");

            EventBus.Subscribe<SendSavePlayerDataEvent>(e => SaveData());
            
            playerDataDontChangableStats = _playerScrObj.StaticPlayerStats;
            
            playerDataStats = _playerScrObj.PlayerDataStats.Clone();

            //var loadData = LoadData();

            //if (loadData != null )
            //{
             //   playerDataStats = loadData;
           // }
            //else
           // {
               
           // }
            Debug.Log($"sila = {playerDataStats.strength}, max hit point = {playerDataStats.startMaxHitPoint}");
        }

        #region SaveData

        private void SaveData()
        {
            Debug.Log("Save data");
            
            string data = JsonUtility.ToJson(playerDataStats);
            
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(filePath))
            {
                File.WriteAllText(filePath, data);
            }
        }

        private PlayerDataStats LoadData()
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    string data = File.ReadAllText(filePath);

                    if (!string.IsNullOrEmpty(data))
                    {
                        PlayerDataStats loadData = JsonUtility.FromJson<PlayerDataStats>(data);
                        Debug.Log($"loadData = {loadData != null}");
                        return loadData;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return null;
        }

        private void OnApplicationQuit()
        {
            EventBus.Unsubscribe<SendSavePlayerDataEvent>(e => SaveData());
        }

        #endregion
        
        #region Getters

        public PlayerDataDontChangableStats GetPlayerDataStaticStats() => playerDataDontChangableStats;

        public PlayerDataStats GetPlayerDataStats() => playerDataStats;

        #endregion
    }
}