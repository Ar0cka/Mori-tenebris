using Actors.Enemy.Movement.Service;
using Zenject;

namespace Scripts.Zenject.GlobalServices
{
    public class InjectAIServices : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DetectedPlayerService>().AsSingle();
        }
    }
}