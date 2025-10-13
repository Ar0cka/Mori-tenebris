using DefaultNamespace.Enums;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
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