using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class InjectItemUiFactory : MonoInstaller
    {
        [SerializeField] private string itemPrefabPath;

        public override void InstallBindings()
        {
            Container.Bind<IItemUIFactory>().To<ItemUIFactory>()
                .AsSingle().WithArguments(itemPrefabPath);
        }
    }
}