using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectProjectSpawnObject : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISpawnProjectObject>().To<DynamicFactory>().AsSingle().NonLazy();
        }
    }

    public interface ISpawnProjectObject
    {
        GameObject Create(GameObject gameObject, Transform parent);
    }

    public class DynamicFactory : ISpawnProjectObject
    {
        private readonly DiContainer _diContainer;

        public DynamicFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public GameObject Create(GameObject prefab, Transform parent)
        {
            return _diContainer.InstantiatePrefab(prefab, parent);
        }
    }
}