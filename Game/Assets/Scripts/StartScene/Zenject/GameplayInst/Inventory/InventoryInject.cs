using Player.Inventory.InventoryInterface;
using PlayerNameSpace.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InventoryInject : MonoInstaller
    {
        [SerializeField] private GameObject _inventory;
        public void InstallInventory()
        {
            Container.Bind<Inventory>().FromComponentInNewPrefab(_inventory).AsSingle().NonLazy();
            
            Container.Bind<IInventoryAdder>().FromResolve();
            Container.Bind<IInventoryRemove>().FromResolve();
            Container.Bind<IInventorySearch>().FromResolve();
        }
    }
}