namespace Actors.Enemy.AttackStateMachine
{
    public interface IAttackState
    {
        public float StateDelay { get; }
        public bool Apply();
        public bool Action();
        public bool EndAction(float dt);
    }
}