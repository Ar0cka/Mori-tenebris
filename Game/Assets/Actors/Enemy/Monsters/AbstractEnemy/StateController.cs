namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public class StateController
    {
        public bool IsHit { get; private set; }
        public bool IsDeath { get; private set; }
        public bool IsAttacking { get; private set; }
        
        public void ChangeStateHit(bool isHit)
        {
            IsHit = isHit;
        }

        public void ChangeStateDeath(bool isDeath)
        {
            IsDeath = isDeath;
        }

        public void ChangeStateAttack(bool isAttacking)
        {
            IsAttacking = isAttacking;
        }

        public bool CanAttack()
        {
            return !IsHit && !IsAttacking && !IsDeath;
        }

        public bool CanMove()
        {
            return !IsAttacking && !IsDeath;
        }
    }
}