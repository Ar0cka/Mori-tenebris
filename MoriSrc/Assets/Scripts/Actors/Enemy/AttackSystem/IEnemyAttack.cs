namespace Actors.Enemy.AttackSystem
{
    public interface IEnemyAttack
    {
        public float CurrentCooldown { get; }
        public void CooldownAttack();
        public float BeginAttack();
        public float ExecuteHit();
        public bool EndAttack();
        public bool CheckRadius();
    }
}