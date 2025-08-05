using Actors.NPC.DialogSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.NPC.NpcStateSystem
{
    public abstract class NPCAbstractSpecialPanel : MonoBehaviour, INpcSpecialPanel
    {
        [SerializeField] protected GameObject specialPanel;
        [SerializeField] protected SpecialPanelType currentPanelType;

        protected DialogFSM DialogFsm;
        
        public virtual void InitializeSpecialPanel(DialogFsmRealize dialogFsmRealize)
        {
            DialogFsm = dialogFsmRealize.GetDialogFsm();
            DialogFsm.OnOpenSpecialPanel += CheckPanelType;
        }
        
        public virtual void OpenPanel()
        {
            specialPanel.SetActive(true);
        }
        public void ClosePanel()
        {
            specialPanel.SetActive(false);
        }
        public virtual void CheckPanelType(SpecialPanelType neededPanelType)
        {
            if (currentPanelType == neededPanelType)
            {
                OpenPanel();
            }
        }
    }

    public interface INpcSpecialPanel
    {
        void OpenPanel();
        void ClosePanel();
        void CheckPanelType(SpecialPanelType neededPanelType);
    }

    public enum SpecialPanelType
    {
        Craft, 
        Shop,
        Repair
    }
}