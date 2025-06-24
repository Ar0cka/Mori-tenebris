using Actors.Enemy.Data.Scripts;
using UnityEngine;

namespace Actors.Enemy.AttackStateMachine.State
{
    public class BeginState : IAttackState
    {
        private AnimAttackSettings _animAttackSettings;
        private Animator _animator;

        public float StateDelay { get; private set; }
        private float _currentTime;
        
        public BeginState(Animator animator, AnimAttackSettings attackSettings, float exitStateDelay)
        {
            _animAttackSettings = attackSettings;
            _animator = animator;
            StateDelay = exitStateDelay;
        }
        
        public bool Apply()
        {
            if (_animAttackSettings == null || _animator == null) return false;
            
            return true;
        }

        public bool Action()
        {
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName(_animAttackSettings.nameTrigger)) return false;
            
            _animator.SetTrigger(_animAttackSettings.nameTrigger);
            
            return true;
        }

        public bool EndAction(float dt)
        {
            if (_currentTime <= 0)
            {
                _currentTime -= dt;
                _currentTime = StateDelay;
                return true;
            }
            
            return false;
        }
    }
}