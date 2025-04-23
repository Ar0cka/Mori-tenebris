using System;
using UnityEngine;

namespace DefaultNamespace.PlayerStatsOperation
{
    public class SaveAllPlayerData : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            EventBus.Publish(new SendSavePlayerDataEvent());
        }
    }
    
    public class SendSavePlayerDataEvent {}
}