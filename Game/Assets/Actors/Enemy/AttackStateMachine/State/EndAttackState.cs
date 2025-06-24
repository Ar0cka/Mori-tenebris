using UnityEngine;

namespace Actors.Enemy.AttackStateMachine.State
{
    public class EndAttackState : IAttackState
    {
        private Animator _animator;
        private string _nameTrigger;
        private float _currentDt;
        
        public float StateDelay { get; private set; }
        
        public EndAttackState(float delay, Animator animator, string nameAttack)
        {
            _animator = animator;
            _nameTrigger = nameAttack;
            StateDelay = delay;
        }
        
        public bool Apply()
        {
            if (_animator == null) return false;

            _currentDt = StateDelay;
            return true;
        }
        
        public bool Action()
        {
            return true;
        }
        
        public bool EndAction(float dt)
        {
            _currentDt -= dt;

            if (_currentDt <= 0)
            {
                _animator.ResetTrigger(_nameTrigger);
                return true;
            }
            return false;
        }
    }
}