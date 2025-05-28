using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class SceneSpawnFactory : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISpawnSceneObject>().To<SceneDynamicSpawn>().AsSingle();
        }
    }

    public interface ISpawnSceneObject
    {
        GameObject Create(GameObject gameObject);
    }

    public class SceneDynamicSpawn : ISpawnSceneObject
    {
        private readonly DiContainer _diContainer;

        public SceneDynamicSpawn(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public GameObject Create(GameObject gameObject)
        {
            return _diContainer.InstantiatePrefab(gameObject);
        }
    }
}