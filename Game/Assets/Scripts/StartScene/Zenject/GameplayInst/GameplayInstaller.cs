using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Zenject;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private InjectPlayerData injectPlayerData;
    [SerializeField] private InjectSpawnObject spawnObject;
    [SerializeField] private InventoryInject inventoryInject;

    public override void InstallBindings()
    {
        injectPlayerData.InstallPlayerData();
        inventoryInject.InstallInventory();
        spawnObject.InstallFactorySpawnItems();
    }
}
