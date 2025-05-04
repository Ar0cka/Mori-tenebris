using EventBusNamespace;
using UnityEngine;

namespace PlayerNameSpace
{
    public class SaveAllPlayerData : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            EventBus.Publish(new SendSavePlayerDataEvent());
        }
    }
}