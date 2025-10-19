using System;
using UnityEngine;

namespace Scripts.CameraScripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private float cameraSpeed;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            MoveCameraToPlayer();
        }

        private void MoveCameraToPlayer()
        {
            Vector3 playerDirection = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, playerDirection, cameraSpeed * Time.deltaTime);
        }
    }
}