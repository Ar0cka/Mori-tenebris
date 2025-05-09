using Actors.Enemy.Pathfinder;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectGrid : MonoInstaller
    {
        [SerializeField] private GridCreater girdCreater;
        public override void InstallBindings()
        {
            Container.Bind<GridCreater>().FromInstance(girdCreater);
        }
    }
}