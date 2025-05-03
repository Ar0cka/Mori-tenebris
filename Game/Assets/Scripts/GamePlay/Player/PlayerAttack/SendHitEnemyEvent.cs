namespace Player.PlayerAttack
{
    public class SendHitEnemyEvent
    {
        public int Damage { get; private set; }
        
        public SendHitEnemyEvent(int damage)
        {
            Damage = damage;
        }
    }
}