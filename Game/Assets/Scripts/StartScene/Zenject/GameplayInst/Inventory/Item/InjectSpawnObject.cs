using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectSpawnObject : MonoInstaller
    {
        public void InstallFactorySpawnItems()
        {
            Container.Bind<IItemsFactory>().To<DynamicFactory>().AsSingle().NonLazy();
        }
    }

    public interface IItemsFactory
    {
        GameObject Create(GameObject gameObject, Transform parent);
    }

    public class DynamicFactory : IItemsFactory
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