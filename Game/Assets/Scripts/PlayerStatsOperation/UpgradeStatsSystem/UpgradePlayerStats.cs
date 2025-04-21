using System;
using System.Collections.Generic;
using DefaultNamespace.Interface;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class UpgradePlayerStats : MonoBehaviour, IUpgradeStat
    {
        [Inject] private IGetPlayerStat playerDataStats;

        public void UpgradeStats(string statName)
        {
            if (statName == null)
            {
                Debug.LogError("Stat Name cannot be null");
                return;
            }

            var state = playerDataStats.TryGetStat(statName);

            if (state != null)
            {
                state.UpgradeStats();
            }
        }
    }
}