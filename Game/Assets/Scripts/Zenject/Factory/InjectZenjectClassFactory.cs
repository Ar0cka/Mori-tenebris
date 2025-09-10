using Service;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectZenjectClassFactory : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ZenjectClassFactory>().AsSingle();
        }
    }
}