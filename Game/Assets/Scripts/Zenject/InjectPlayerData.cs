using DefaultNamespace.Interface;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectPlayerData : MonoInstaller
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private UpgradePlayerStats upgradePlayerStats;

        public override void InstallBindings()
        {
            Container.Bind<IGetPlayerStat>().FromInstance(playerData).AsSingle();
            Container.Bind<IUpgradeStat>().FromInstance(upgradePlayerStats).AsSingle();
        }
    }
}