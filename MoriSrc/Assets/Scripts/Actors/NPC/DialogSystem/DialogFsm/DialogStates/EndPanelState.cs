using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;

namespace Actors.NPC.DialogSystem.FSM.DialogStates
{
    public class EndPanelState : DialogState
    {
        public EndPanelState(DialogFSM stateMachine) : base(stateMachine)
        {
           
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            EndAction();
        }

        private void EndAction()
        {
            Fsm.OnLastDialogNode?.Invoke();
            ChangeDialogState<IdleDialogState>();
        }
        
        public override void Update()
        {
            // Конечное состояния, отключаем базовый FixedUpdate
        }
        
    }
}