using Actors.Player.AbstractFSM;
using Actors.Player.Movement.Scripts;
using UnityEngine;

namespace Actors.Player.AttackSystemFSM.States
{
    public class BaseAttackState : State
    {
        private Animator _animator;
        private AttackFSMRealize _attackFsmRealize;
        private FStateMachine _stateMachine;

        public BaseAttackState(FStateMachine fStateMachine, AttackFSMRealize fsmRealize) :
            base(fStateMachine, fsmRealize)
        {
           _attackFsmRealize = fsmRealize;
           _stateMachine = fStateMachine;
        }
    }
}