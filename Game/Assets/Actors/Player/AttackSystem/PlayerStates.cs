namespace Actors.Player.AttackSystem
{
    public class PlayerStates
    {
        public static bool IsAttacking { get; private set; }
        public static bool IsCastingSpell { get; private set; }
        public static bool IsBlocking { get; private set; }
        
        public static bool IsRolling { get; private set; }

        public static bool IsBusy => IsAttacking || IsCastingSpell || IsBlocking || IsRolling;
        
        public static void UpdateAttackState(bool attackState)
        {
            IsAttacking = attackState;
        }

        public static void UpdateCastingSpell(bool castingSpell)
        {
            IsCastingSpell = castingSpell;
        }

        public static void UpdateBlocking(bool blocking)
        {
            IsBlocking = blocking;
        }

        public static void UpdateRolling(bool rolling)
        {
            IsRolling = rolling;
        }
    }
}