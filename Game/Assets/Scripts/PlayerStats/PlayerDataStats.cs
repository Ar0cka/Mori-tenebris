using ModestTree;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [System.Serializable]
    public class PlayerDataStats
    {
        public string nameState;
        public int currentStatLevel;
        public float currentCountStats;
        public int maxLevelStats;
        public float countUpgradeStats;

        public void UpgradeStats()
        {
            if (currentStatLevel < maxLevelStats)
            {
                currentStatLevel++;
                currentCountStats += countUpgradeStats;
                Debug.Log($"currentStatLevel: {currentStatLevel}, currentCountStats: {currentCountStats}");
            }
            else
            {
                Debug.Log("Max level stats reached");
            }
        }

        public PlayerDataStats Clone()
        {
            return new PlayerDataStats()
            {
                nameState = nameState,
                currentStatLevel = currentStatLevel,
                currentCountStats = currentCountStats,
                maxLevelStats = maxLevelStats,
                countUpgradeStats = countUpgradeStats
            };
        }
    }
}