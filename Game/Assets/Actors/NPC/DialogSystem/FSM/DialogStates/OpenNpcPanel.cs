using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using StateMachin.States;

namespace Actors.NPC.DialogSystem.FSM.DialogStates
{
    public class OpenNpcPanel : DialogState
    {
        public OpenNpcPanel(DialogFSM fsm) : base(fsm)
        {
            Fsm = fsm;
        }

        public override void Enter(DialogNode node)
        {
            base.Enter(node);
            OpenDialog();
        }

        private void OpenDialog()
        {
            Fsm.OnOpenSpecialPanel?.Invoke(CurrentDialogNode.SpecialPanelSettings.specialPanelType);
            ChangeDialogState<IdlePanelState>();
        }

        public override void Update()
        {
            //Заглушка
        }
    }
}