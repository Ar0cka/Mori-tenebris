using Actors.Enemy.Data.Scripts;

namespace Actors.Enemy.Monsters.AbstractEnemy
{
    public interface IAttackState
    {
        void Apply(AnimAttackSettings animAttackSettings);
        void Action();
        void EndAction();
    }
}