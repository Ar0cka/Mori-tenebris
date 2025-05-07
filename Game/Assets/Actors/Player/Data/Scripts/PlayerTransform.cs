using UnityEngine;

namespace PlayerNameSpace
{
    public static class PlayerTransform
    {
        public static Transform PlayerPosition {get; private set;}

        public static void PlayerInitialize(Transform playerTransform)
        {
            PlayerPosition = playerTransform;
        }
    }
}