using DefaultNamespace.Enums;
using NegativeEffects;

namespace PlayerNameSpace
{
    public interface IHitPlayer
    {
        public int CurrentHitPoint { get; }

        void TakeDamage(int damage, DamageType damageType);
    }
}