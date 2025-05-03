using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using Zenject;
using PlayerNameSpace.InventorySystem;

namespace DefaultNamespace.Zenject.Inventory
{
    public class InjectInventory : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InventoryLogic>().AsSingle().NonLazy();
            Container.Bind<IInventoryAdder>().To<InventoryLogic>().FromResolve();
            Container.Bind<IInventoryRemove>().To<InventoryLogic>().FromResolve();
            Container.Bind<IInventorySearch>().To<InventoryLogic>().FromResolve();
        }
    }
}