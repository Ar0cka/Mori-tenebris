using DefaultNamespace.Interface;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI
{
    public class UpgradeStatButton : MonoBehaviour
    {
        [Header("NameStat")]
        [SerializeField] private string statName;
        
        [Inject] private IUpgradeStat _upgradeStat;
        
        public void UpdateStats()
        {
            _upgradeStat.UpgradeStats(statName);
        }
    }
}