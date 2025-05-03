using UnityEngine;

namespace Zenject.Infrastructure
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private SaveSystemInstaller globalOperations;
        public override void InstallBindings()
        {
            globalOperations.SaveOperationsInstall();
        }
    }
}