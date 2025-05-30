using Unity.VisualScripting;
using UnityEngine;

namespace Actors.Player.Movement.Scripts
{
    [CreateAssetMenu(fileName = "MoveColliderSettings", menuName = "Player/ColliderSettings", order = 0)]
    public class MovementOffsetScr : ScriptableObject
    {
        [Header("ColliderOffset")]
        [SerializeField] private Vector2 moveRightOffset;
        [SerializeField] private Vector2 moveLeftOffset;
        
        public Vector2 MoveRightOffset => moveRightOffset;
        public Vector2 MoveLeftOffset => moveLeftOffset;
    }
}