namespace Player.PlayerAttack
{
    public class SendBaseAttackEvent
    {
        public int CurrentAttack { get; }

        public SendBaseAttackEvent(int currentAttack)
        {
            CurrentAttack = currentAttack;
        }
    }
}