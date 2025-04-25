using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        //[SerializeField] private PlayerUIManager playerUIManager;
        [Inject] private Health _health;
        
        private void Awake()
        {
            playerData.Initialize();
            //playerUIManager.Initialize();
            _health.Initialize();
        }
    }
}