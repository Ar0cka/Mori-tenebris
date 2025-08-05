namespace Actors.NPC.DialogSystem.DataScripts
{
    public interface IDialogCondition
    {
        ConditionType CurrentConditionType { get; set; }
        int ReputationNum { get; set; }
    }

    public enum ConditionType
    {
        Default,
        Quest,
        Aggressive,
        Friendly,
    }
    public enum DialogActionType
    {
        None,
        OpenPanel,
        Dialog,
        Quest
    }
}