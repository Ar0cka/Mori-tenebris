using System;
using DefaultNamespace.PlayerStatsOperation;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        private void Awake()
        {
            playerData.Initialize(); ;
        }
    }
}