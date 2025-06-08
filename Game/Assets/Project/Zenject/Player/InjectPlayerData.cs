using Actors.Player.AttackSystem;
using PlayerNameSpace;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectPlayerData : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerData>().AsSingle().NonLazy();
            
            Container.Bind<IGetPlayerStat>().To<PlayerData>().FromResolve();
            
            Container.Bind<IUpgradeStat>().To<UpgradeStat>().AsSingle();
            
            Container.Bind<Armour>().AsSingle();
            Container.Bind<IEquipAndUnEquipItem>().To<Armour>().FromResolve();

            Container.Bind<Health>().AsSingle().NonLazy();

            Container.Bind<ITakeDamage>().To<Health>().FromResolve();
            Container.Bind<IRegenerationHealth>().To<Health>().FromResolve();
            
            Container.Bind<Stamina>().AsSingle().NonLazy();
            Container.Bind<ISubtractionStamina>().To<Stamina>().FromResolve();
            Container.Bind<IRegenerationStamina>().To<Stamina>().FromResolve();
            
            Container.Bind<DamageSystem>().AsSingle().NonLazy();
        }
    }
}