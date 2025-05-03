using DefaultNamespace.Enums;
using System;
using EventBusNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerNameSpace
{
    [Serializable]
    public class PlayerDataStats
    {
        #region Data

        [Header("Attributes")]
        [SerializeField] private int strength;
        [SerializeField] private int agility;
        [SerializeField] private int vitality;
        [SerializeField] private int lucky;

        [Header("Stats")]
        [SerializeField] private int startMaxHitPoint;
        [SerializeField] private int startMaxStamina;

        [Header("Level")]
        [SerializeField] private int level;
        [SerializeField] private int currentExperience;
        [SerializeField] private int experienceForNextLevel;
        [SerializeField] private int maxLevel;
        [SerializeField] private int upgradeCount;
        [SerializeField] private int countUpgradeAdd;

        [Header("RegenerationSettings")] 
        [SerializeField] private float speedRegenerationHealth;
        [SerializeField] private float speedRegenerationStamina;
        
        
        #endregion

        #region Getters

        public int Strength => strength;
        public int Agility => agility;
        public int Vitality => vitality;
        public int Lucky => lucky;
        public int Level => level;
        public int CurrentExperience => currentExperience;
        public int ExperienceForNextLevel => experienceForNextLevel;
        public int UpgradeTokenCount => upgradeCount;
        public float SpeedRegenerationHealth => speedRegenerationHealth;
        public float SpeedRegenerationStamina => speedRegenerationStamina;

        #endregion
        
        public void UpLevel(int experience)
        {
            currentExperience += experience;

            if (CheckInMaxLevel())
            {
                currentExperience = experienceForNextLevel;
                return;
            }
            
            while (currentExperience > experienceForNextLevel)
            {
                currentExperience -= experienceForNextLevel;
                upgradeCount += countUpgradeAdd;
                level++;
                UpdateExpForNextLevel();
                EventBus.Publish(new SendUIUpdateExpSystem());
            }
        }

        public bool CheckInMaxLevel()
        {
            return level > maxLevel;
        }
        
        private void UpdateExpForNextLevel()
        {
            experienceForNextLevel *= 2;
        }
        
        public void UpgradeStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.Strength:
                    strength++;
                    break;
                case StatType.Agility:
                    agility++;
                    break;
                case StatType.Vitality:
                    vitality++;
                    break;
                case StatType.Lucky:
                    lucky++;
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
                level = level,
                maxLevel = maxLevel,
                currentExperience = currentExperience,
                experienceForNextLevel = experienceForNextLevel,
                upgradeCount = upgradeCount,
                countUpgradeAdd = countUpgradeAdd, 
                speedRegenerationHealth = speedRegenerationHealth,
                speedRegenerationStamina = speedRegenerationStamina
            };
        }
    }
}

