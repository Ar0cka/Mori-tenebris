namespace Enemy.StatSystems.DamageSystem
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