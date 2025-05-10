using Actors.Enemy.Pathfinder;
using Actors.Enemy.Pathfinder.Interface;
using UnityEngine;
using Zenject;


namespace DefaultNamespace.Zenject
{
    public class InjectPathFind : MonoInstaller
    {
        [SerializeField] private PathfinderSystem pathfinderSystem;

        public override void InstallBindings()
        {
            Container.Bind<IPathFind>().FromInstance(pathfinderSystem).AsSingle();
        }
    }
}