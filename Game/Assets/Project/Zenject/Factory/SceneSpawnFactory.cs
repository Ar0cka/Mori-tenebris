using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class SceneSpawnFactory : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISpawnSceneObject>().To<SceneSyDynamicSpawn>().AsSingle();
        }
    }

    public interface ISpawnSceneObject
    {
        GameObject Create(GameObject gameObject);
    }

    public class SceneSyDynamicSpawn : ISpawnSceneObject
    {
        private readonly DiContainer _diContainer;

        public SceneSyDynamicSpawn(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public GameObject Create(GameObject gameObject)
        {
            return _diContainer.InstantiatePrefab(gameObject);
        }
    }
}