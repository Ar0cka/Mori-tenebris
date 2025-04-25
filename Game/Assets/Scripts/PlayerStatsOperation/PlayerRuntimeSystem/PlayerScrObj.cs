using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerNameSpace
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerStats")]
    public class PlayerScrObj : ScriptableObject
    {
        [SerializeField] private PlayerDataStats playerDataStats;
        public PlayerDataStats PlayerDataStats => playerDataStats;

        [SerializeField] private PlayerStaticData playerDefaultStat;
        public PlayerStaticData StaticPlayerStats => playerDefaultStat;
    }
}
