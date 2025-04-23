using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerStats")]
public class PlayerScrObj : ScriptableObject
{
    [SerializeField] private PlayerDataStats playerDataStats;
    public PlayerDataStats PlayerDataStats => playerDataStats;

    [SerializeField] private PlayerDataDontChangableStats playerDefaultStat;
    public PlayerDataDontChangableStats StaticPlayerStats => playerDefaultStat;
}

[Serializable]
public class PlayerDataStats
{
    public int strength;
    public int agility;
    public int vitality;
    public int lucky;

    public int startMaxHitPoint;
    public int startMaxStamina;

    public int countUpgradeFroLevel;

    public void UpgradeStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Strength:
                strength += countUpgradeFroLevel;
                break;  
            case StatType.Agility:
                agility += countUpgradeFroLevel;
                break;
            case StatType.Vitality:
                vitality += countUpgradeFroLevel;
                break;
            case StatType.Lucky:
                lucky += countUpgradeFroLevel;
                break;
            default:
                Debug.LogError("Dont find this type stat");
                break;
        }
        
        Debug.Log($"Sila after upgrade = {strength}");
    }
    
    public PlayerDataStats Clone()
    {
        return new PlayerDataStats()
        {
            strength = strength,
            agility = agility,
            vitality = vitality, 
            lucky = lucky,
            startMaxStamina = startMaxStamina,
            startMaxHitPoint = startMaxHitPoint,
            countUpgradeFroLevel = countUpgradeFroLevel
        };
    }
}