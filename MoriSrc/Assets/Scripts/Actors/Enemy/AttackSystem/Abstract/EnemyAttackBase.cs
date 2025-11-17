using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Movement.Base.Service;
using FiniteStateMachine;
using UnityEngine;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public class EnemyAttackBase<TConfig, TAttackContext>: IEnemyAttack 
        where TConfig : AttackConfig 
        where TAttackContext : AttackContext
    {
        protected TConfig Config;
        protected TAttackContext Ctx;

        public float CurrentCooldown { get; private set; }
        
        public EnemyAttackBase(TConfig config, TAttackContext ctx)
        {
            Config = config;
            Ctx = ctx;
        }
        
        public virtual float BeginAttack()
        {
            throw new System.NotImplementedException();
        }
        public virtual float ExecuteHit()
        {
            throw new System.NotImplementedException();
        }
        public virtual bool EndAttack()
        {
            CurrentCooldown = Config.cooldownAttack;
            return true;
        }
        public virtual void CooldownAttack()
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= Time.deltaTime;
            }
        }
        public virtual bool SetAnimation(string animationName, float currentSize)
        {
            if (CurrentCooldown > 0)
                return false;

            var stateInfo = Ctx.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(animationName))
                return false;
            
            Ctx.SpriteRenderer.flipX = currentSize > 0;
            Ctx.Animator.SetTrigger(animationName);

            return true;
        }

        public bool CheckRadius()
        {
            var targetInZone =
                Ctx.DetectedPlayerService.DetectedTarget(Config.attackDistance, Config.hitLayer, Ctx.Rb2D.position);
            
            if (targetInZone == Vector2.zero)
                return false;
            
            return true;
        }
    }

    public class AttackContext
    {
        public readonly Animator Animator;
        public readonly Rigidbody2D Rb2D;
        public readonly Transform HitPoint;
        public readonly SpriteRenderer SpriteRenderer;
        public readonly DetectedPlayerService DetectedPlayerService;

        public AttackContext(Animator animator, Transform hitPoint, 
            SpriteRenderer spriteRenderer, DetectedPlayerService detectedPlayerService, Rigidbody2D rb2D)
        {
            Animator = animator;
            HitPoint = hitPoint;
            SpriteRenderer = spriteRenderer;
            DetectedPlayerService = detectedPlayerService;
            Rb2D = rb2D;
        }
    }
}