using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using Zenject;


public class SaveSystemInstaller : MonoInstaller
{
    public void SaveOperationsInstall()
    {
        Container.Bind<ISaveAndLoad>().To<SaveAndLoadSystem>().AsSingle().NonLazy();
    }
}