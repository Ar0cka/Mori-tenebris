using System;
using DefaultNamespace.Enums;

namespace DefaultNamespace.PlayerStatsOperation.StatUpgrade
{
    public interface IUpgradeStat
    {
        void Upgrade(StatType statType);
    }
}