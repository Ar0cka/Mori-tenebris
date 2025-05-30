using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors.Player.AttackSystem
{
    public class AbstractAttack : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        [Header("SidesValue")] 
        [SerializeField] protected string nameSideParameters;
        [SerializeField] protected int leftSide;
        [SerializeField] protected int rightSide;
        
        protected Dictionary<int, string> _queueAttackDictionary = new Dictionary<int, string>();
        
        protected float Cooldown;
        protected string CurrentAnimation;
        protected bool isBusy;
       
        protected virtual void Awake()
        {
            if (ValidateComponents())
            {
                enabled = false;
                return;
            }
        }
        
        protected void SetAnimation(string animationName)
        {
            animator.SetFloat(nameSideParameters, GetAnimationVector());
            animator.SetTrigger(animationName);
        }

        private int GetAnimationVector()
        {
            return spriteRenderer.flipX ? leftSide : rightSide;
        }
        
        protected bool ValidateComponents()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (animator == null) animator = GetComponentInParent<Animator>();
            
            
            return spriteRenderer == null || animator == null;
        }
    }
}