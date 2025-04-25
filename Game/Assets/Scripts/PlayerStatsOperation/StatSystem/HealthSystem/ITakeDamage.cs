using DefaultNamespace.Enums;

namespace PlayerNameSpace
{
    public interface ITakeDamage
    {
        public int CurrentHitPoint { get; }

        void TakeDamage(int damage, DamageType damageType);
    }
}