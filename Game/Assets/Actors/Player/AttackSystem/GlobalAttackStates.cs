namespace Actors.Player.AttackSystem
{
    public class GlobalAttackStates
    {
        public static bool IsAttacking { get; private set; }
        public static bool IsCastingSpell { get; private set; }
        public static bool IsBlocking { get; private set; }

        public static bool IsBusy => IsAttacking || IsCastingSpell || IsBlocking;
        
        public static void UpdateAttackState(bool attackState)
        {
            GlobalAttackStates.IsAttacking = attackState;
        }
    }
}