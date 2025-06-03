using Actors.Enemy.Data.Scripts;
using DefaultNamespace.Enums;

namespace Enemy.StatSystems.DamageSystem
{
    public class EnemyDamage
    {
        public int Damage { get; private set; }
        public DamageType DamageType { get; private set; }

        public EnemyDamage(int damage = 0, DamageType damageType = DamageType.Physic)
        {
            Damage = damage;
            DamageType = damageType;
        }

        public void DamageUpdate(AttackConfig attackConfig)
        {
            Damage = attackConfig.damage;
            DamageType = attackConfig.damageType;
        }
    }
}