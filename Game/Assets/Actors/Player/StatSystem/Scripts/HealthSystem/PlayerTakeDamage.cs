using DefaultNamespace.Enums;
using NegativeEffects;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerTakeDamage : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private PlayerEffectController playerEffectController;
        
        [Inject] private IHitPlayer _hitPlayer;

        public int GetCurrentHitPoint()
        {
            return _hitPlayer.CurrentHitPoint;
        }

        public void TakeHit(int damage, DamageType damageType)
        {
            _hitPlayer.TakeDamage(damage, damageType);
        }

        public void AddEffect(EffectScrObj effect)
        {
            playerEffectController.AddEffect(effect);
        }
    }

    public interface ITakeDamage
    {
        int GetCurrentHitPoint();
        void TakeHit(int damage, DamageType damageType);
        void AddEffect(EffectScrObj effect);
    } 
}