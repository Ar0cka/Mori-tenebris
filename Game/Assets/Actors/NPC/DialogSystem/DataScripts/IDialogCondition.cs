namespace Actors.NPC.DialogSystem.DataScripts
{
    public interface IDialogCondition
    {
        ConditionType CurrentConditionType { get; set; }
        int ReputationNum { get; set; }
    }

    public enum ConditionType
    {
        Quest,
        Aggressive,
        Friendly,
        Default,
    }
    public enum DialogActionType
    {
        OpenPanel,
        Dialog,
        Quest
    }
}