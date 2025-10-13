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
            Fsm.OnOpenShop?.Invoke(CurrentDialogNode.SpecialPanelSettings.specialPanelType, Fsm.DialogContext.InventoryPanel, Fsm.DialogContext.Wallet);
            ChangeDialogState<IdlePanelState>();
        }

        public override void Update()
        {
            //Заглушка
        }
    }
}