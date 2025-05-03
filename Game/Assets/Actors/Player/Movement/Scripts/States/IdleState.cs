using UnityEngine;

namespace StateMachin.States
{
    public class IdleState : State
    {
        private bool isRunning;
        
        public IdleState(FStateMachine fsm, StateMachineRealize stateMachineRealize) : base(fsm, stateMachineRealize)
        {
            
        }
        
        public override void UpdateLogic()
        {
            base.UpdateLogic();
            Vector2 inputVector = GetInputFromKeyboard();

            isRunning = inputVector.sqrMagnitude != 0;

            if (isRunning)
            {
                FStateMachine.ChangeState<MovementState>();
            }
        }

        private Vector2 GetInputFromKeyboard()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}