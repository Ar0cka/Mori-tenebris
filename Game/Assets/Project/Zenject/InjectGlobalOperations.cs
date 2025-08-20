using DefaultNamespace;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using Items;
using Zenject;


public class InjectGlobalOperations : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISaveAndLoad>().To<SaveAndLoadSystem>().AsSingle().NonLazy();
        Container.Bind<ItemRouterService>().AsSingle().NonLazy();
        Container.Bind<PanelController>().AsSingle();
    }
}