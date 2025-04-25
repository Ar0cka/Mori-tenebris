using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using Zenject;


public class InjectGlobalOperations : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISaveAndLoad>().To<SaveAndLoadSystem>().AsSingle().NonLazy();
    }
}