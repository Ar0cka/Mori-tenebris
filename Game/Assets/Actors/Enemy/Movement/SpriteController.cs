using Actors.Player.Movement.Scripts;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    public abstract class SpriteController : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Animator animator;
        
        
        public virtual void SetModelSettings(Vector2 moveDirection)
        {
            SetAnimationState(moveDirection);
            
            if (moveDirection != Vector2.zero) 
                SetFlipState(moveDirection);
        }

        public virtual void SetAnimationState(Vector2 moveDirection)
        {
            animator.SetBool("Walk", moveDirection.magnitude != 0);
        }

        public virtual void SetFlipState(Vector2 moveDirection)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }

        public virtual void SetColliderSettings(Vector2 moveDirection)
        {
        }
    }
}