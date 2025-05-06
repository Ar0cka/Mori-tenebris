namespace Actors.Enemy.Stats.StatSystems.DamageSystem
{
    public class EnemyDamage
    {
        public int Damage { get; private set; }

        public EnemyDamage(int damage)
        {
            Damage = damage;
        }
    }
}