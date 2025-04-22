using DefaultNamespace.Events;
using DefaultNamespace.Interface;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using DefaultNamespace.PlayerStatsOperation.StatSystem;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using DefaultNamespace.PlayerStatsOperation.StatUpgrade;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectPlayerData : MonoInstaller
    {
        [SerializeField] private string nameHitPointStat;
        
        [SerializeField] private PlayerData playerData;

        public override void InstallBindings()
        {
            Container.Bind<IGetPlayerStat>().FromInstance(playerData).AsSingle();
            Container.Bind<IUpgradeStat>().To<UpgradeStat>().AsSingle();
            
            Container.Bind<Armour>().FromNew().AsSingle();
            Container.Bind<IEquipAndUnEquipItem>().To<Armour>().FromResolve();

            Container.Bind<Health>().AsSingle().WithArguments(nameHitPointStat).NonLazy();

            Container.Bind<ITakeDamage>().To<Health>().FromResolve();
            Container.Bind<IRegeneration>().To<Health>().FromResolve();
            Container.Bind<IGetSubscribeInDieEvent>().To<Health>().FromResolve();
        }
    }
}