using System;
using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.PlayerStatsOperation.StatUpgrade
{
    public class UpgradeStat : IUpgradeStat
    {
        [Inject] private IGetPlayerStat _playerStat;
        public void Upgrade(StatType statType)
        {
            _playerStat.GetPlayerDataStats().UpgradeStat(statType);
            EventBus.Publish(new SendUpdateStatEvent());
        }
    }
    
    public class SendUpdateStatEvent{}
}