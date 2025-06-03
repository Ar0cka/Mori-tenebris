using Actors.Enemy.Movement;
using UnityEngine;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeSpriteConroller : SpriteController
    {
        public override void SetFlipState(Vector2 moveDirection)
        {
            spriteRenderer.flipX = moveDirection.x > 0;
        }
    }
}