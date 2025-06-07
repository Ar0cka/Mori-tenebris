using UnityEngine;

namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public class StateController
    {
        public bool IsHit { get; private set; }
        public bool IsDeath { get; private set; }
        public bool IsAttacking { get; private set; }
        
        public bool IsInterruptAttack { get; private set; }
        
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
        
        public void ChangeInterruptAttack(bool canInterrupt)
        {
            IsInterruptAttack = canInterrupt;
        }

        public void Jump(bool isAttacking, bool isInterrupt)
        {
            IsAttacking = isAttacking;
            IsInterruptAttack = isInterrupt;
        }

        public bool CanAttack()
        {
            Debug.Log($"IsHit: {IsHit}, IsDeath: {IsDeath}, IsAttacking: {IsAttacking}");
            return !IsHit && !IsAttacking && !IsDeath;
        }
        
        public bool CanMove()
        {
            return !IsAttacking && !IsDeath;
        }

        public bool CanHit()
        {
            Debug.Log("is interrupt = " + IsInterruptAttack);
            return !IsInterruptAttack;
        }
    }
}