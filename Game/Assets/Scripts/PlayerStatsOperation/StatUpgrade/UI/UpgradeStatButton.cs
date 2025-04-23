using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation.StatUpgrade;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI
{
    public class UpgradeStatButton : MonoBehaviour
    {
        [SerializeField] private StatType statType;
        [Inject] private IUpgradeStat _upgradeStat;

        public void Upgrade()
        {
            _upgradeStat.Upgrade(statType);
        }
    }
}