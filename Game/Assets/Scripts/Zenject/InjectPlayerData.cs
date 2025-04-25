using PlayerNameSpace;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectPlayerData : MonoInstaller
    {
        [SerializeField] private PlayerData playerData;

        public override void InstallBindings()
        {
            Container.Bind<IGetPlayerStat>().FromInstance(playerData).AsSingle().NonLazy();
            
            Container.Bind<IUpgradeStat>().To<UpgradeStat>().AsSingle();
            
            Container.Bind<Armour>().FromNew().AsSingle();
            Container.Bind<IEquipAndUnEquipItem>().To<Armour>().FromResolve();

            Container.Bind<Health>().AsSingle().NonLazy();

            Container.Bind<ITakeDamage>().To<Health>().FromResolve();
            Container.Bind<IRegeneration>().To<Health>().FromResolve();
        }
    }
}