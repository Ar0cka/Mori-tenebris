namespace Actors.NPC.DialogSystem.DataScripts
{
    public interface IDialogCondition
    {
        ConditionType CurrentConditionType { get; set; }
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
        Default,
        OpenPanel,
        Dialog,
        Quest
    }
}