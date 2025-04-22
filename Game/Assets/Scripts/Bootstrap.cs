using System;
using DefaultNamespace.Events;
using DefaultNamespace.PlayerStatsOperation;
using DefaultNamespace.PlayerStatsOperation.StatSystem;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [Inject] private Health _health;
        
        private void Awake()
        {
            playerData.Initialize();
            _health.Initialize();
        }
    }
}