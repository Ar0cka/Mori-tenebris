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
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Inventory inventory;

        public override void InstallBindings()
        {
            Container.Bind<IGetPlayerStat>().FromInstance(playerData).AsSingle().NonLazy();
            
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

            Container.Bind<IInventoryAdder>().FromInstance(inventory);
            Container.Bind<IInventoryRemove>().FromInstance(inventory);
            Container.Bind<IInventorySearch>().FromInstance(inventory);
        }
    }
}