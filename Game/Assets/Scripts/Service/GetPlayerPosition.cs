using UnityEngine;

namespace PlayerNameSpace
{
    public static class GetPlayerPosition
    {
        private static Transform _playerPosition;

        public static void Initialize(Transform playerPosition)
        {
            _playerPosition = playerPosition;
        }

        public static Transform PlayerPosition()
        {
            return _playerPosition;
        }
    }
}