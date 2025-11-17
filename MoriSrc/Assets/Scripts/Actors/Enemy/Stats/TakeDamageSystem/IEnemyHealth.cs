namespace Actors.Enemy.Stats.Scripts.TakeDamageSystem
{
    public interface IEnemyHealth
    {
        public int CurrentHealth { get; }
        
        void TakeHit(int damage);
        void Heal(int health);
    }
}