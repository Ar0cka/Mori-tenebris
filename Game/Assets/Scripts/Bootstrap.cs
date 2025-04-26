using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerStatsUI playerUIManager;
        [Inject] private Health _health;
        [Inject] private Armour _armour;
        
        private void Awake()
        {
            playerData.Initialize();
            _armour.Initialize();
            _health.Initialize();
            playerUIManager.Initialize();
        }
    }
}