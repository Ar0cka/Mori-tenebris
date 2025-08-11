using Actors.NPC.DialogSystem.DataScripts;

namespace Actors.NPC.DialogSystem.FSM.DialogStates
{
    public class IdlePanelState : DialogState
    {
        public IdlePanelState(DialogFSM fsm) : base(fsm)
        {
            Fsm = fsm;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            Fsm.OnClosePanel += PanelIsClosed;
        }

        private void PanelIsClosed()
        {
            ChangeDialogState<EndDialogState>();
        }

        public override void Update()
        {
            ///Заглушка
        }

        public override void Exit()
        {
            base.Exit();
            Fsm.OnClosePanel -= PanelIsClosed;
        }
    }
}