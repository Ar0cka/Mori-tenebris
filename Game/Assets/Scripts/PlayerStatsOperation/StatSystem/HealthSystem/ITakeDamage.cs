using DefaultNamespace.Enums;

namespace DefaultNamespace.PlayerStatsOperation.StatSystem
{
    public interface ITakeDamage
    {
        public int CurrentHitPoint { get; }

        void TakeDamage(int damage, DamageType damageType);
    }
}