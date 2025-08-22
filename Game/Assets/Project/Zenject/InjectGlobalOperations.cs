using DefaultNamespace;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using Items;
using Service;
using Zenject;


public class InjectGlobalOperations : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISaveAndLoad>().To<SaveAndLoadSystem>().AsSingle();
        Container.Bind<ItemRouterService>().AsSingle();
        Container.Bind<PanelController>().AsSingle();
        Container.Bind<InventoryFillService>().AsSingle();
    }
}