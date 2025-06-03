using Actors.Enemy.Data.Scripts;
using DefaultNamespace.Enums;

namespace Enemy.StatSystems.Armour
{
    public class EnemyArmour
    {
        private float _physicArmour;
        private float _magicArmour;

        public EnemyArmour(EnemyConfig enemyData)
        {
            _physicArmour = enemyData.physicArmour;
            _magicArmour = enemyData.magicArmour;
        }

        public float GetArmour(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    return _physicArmour;
                case DamageType.Magic:
                    return _magicArmour;
            }

            return 0;
        }
    }
}