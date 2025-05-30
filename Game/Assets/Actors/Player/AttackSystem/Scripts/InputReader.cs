using System;
using UnityEngine;

namespace Player.PlayerAttack
{
    public class InputReader : MonoBehaviour
    {
        public event Action<KeyCode> keyPressed;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                keyPressed?.Invoke(KeyCode.Mouse0);
            }
        }
    }
}