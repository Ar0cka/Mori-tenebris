using Actors.Enemy.Movement;
using Actors.Player.Movement.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeSpriteConroller : SpriteController
    {
        [SerializeField] private PolygonCollider2D polygonCollider;
        [SerializeField] private MovementOffsetScr colliderSettings;
        public override void SetFlipState(Vector2 moveDirection)
        {
            spriteRenderer.flipX = moveDirection.x > 0;
            SetColliderSettings(moveDirection);
        }

        public override void SetColliderSettings(Vector2 moveDirection)
        {
            if (polygonCollider == null || colliderSettings == null) return;
            
            polygonCollider.offset = moveDirection.x > 0 ? colliderSettings.MoveRightOffset : colliderSettings.MoveLeftOffset;
        }
    }
}