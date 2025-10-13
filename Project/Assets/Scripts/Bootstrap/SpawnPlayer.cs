using PlayerNameSpace;
using UnityEngine;

namespace Project.Bootstrap
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        
        public void SetStartPosition(Transform playerTransform)
        {
            playerTransform.position = startPoint.position;
            GetPlayerPosition.Initialize(playerTransform);
        }
    }
}