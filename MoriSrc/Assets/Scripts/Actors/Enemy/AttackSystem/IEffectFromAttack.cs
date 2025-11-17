using PlayerNameSpace;

namespace Actors.Enemy.AttackSystem
{
    public interface IEffectFromAttack
    {
        public bool ApplyEffectOnTarget(ITakeDamage target);
    }
}