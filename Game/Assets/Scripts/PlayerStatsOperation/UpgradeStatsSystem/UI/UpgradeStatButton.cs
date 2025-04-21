using DefaultNamespace.Interface;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI
{
    public class UpgradeStatButton : MonoBehaviour
    {
        [Header("NameStat")]
        [SerializeField] private string statName;

        private IUpgradeStat _upgradeStat;
        
        [Inject]
        private void Construct(IUpgradeStat statUpgrade)
        {
            _upgradeStat = statUpgrade;
        }
    
        
        public void UpdateStats()
        {
            _upgradeStat.UpgradeStats(statName);
        }
    }
}