using Player.Inventory;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Zenject
{
    public class ItemUIFactory : IItemUIFactory
    {
        private readonly GameObject _itemPrefab;
        private readonly ISpawnProjectObject _spawnProjectObject;

        [Inject]
        public ItemUIFactory(ISpawnProjectObject spawnProjectObject, string path)
        {
            _spawnProjectObject = spawnProjectObject;
            _itemPrefab = Resources.Load<GameObject>(path);
        }

        public ItemUI CreateItemUI()
        {
            var item = _spawnProjectObject.Create(_itemPrefab);
            return item.GetComponent<ItemUI>();
        }
    }

    public interface IItemUIFactory
    {
        ItemUI CreateItemUI();
    }
}