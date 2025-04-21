using System.Collections.Generic;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using UnityEngine;

namespace DefaultNamespace.PlayerStatsOperation
{
    public class PlayerData : MonoBehaviour, IGetPlayerStat
    {
        [SerializeField] private PlayerScrObj _playerScrObj;

        private List<PlayerDataStats> playerDataStats = new List<PlayerDataStats>();

        private Dictionary<string, PlayerDataStats> PlayerDataStatsList = new Dictionary<string, PlayerDataStats>();
        
        private PlayerDataDontChangableStats playerDataDontChangableStats;
        
        public PlayerDataDontChangableStats GetPlayerDataStaticStats() => playerDataDontChangableStats;

        public void Initialize()
        {
            playerDataDontChangableStats = _playerScrObj.StaticPlayerStats;
            
            foreach (var playerData in _playerScrObj.PlayerDataStats)
            {
                playerDataStats.Add(playerData.Clone());
            }

            foreach (var stat in playerDataStats)
            {
                PlayerDataStatsList.Add(stat.nameState, stat);
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
    }
}