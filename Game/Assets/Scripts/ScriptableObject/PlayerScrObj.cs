using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerStats")]
public class PlayerScrObj : ScriptableObject
{
    [SerializeField] private List<PlayerDataStats> playerDataStats;
    public IReadOnlyList<PlayerDataStats> PlayerDataStats => playerDataStats;

    [SerializeField] private PlayerDataDontChangableStats playerDefaultStat;
    public PlayerDataDontChangableStats StaticPlayerStats => playerDefaultStat;
}