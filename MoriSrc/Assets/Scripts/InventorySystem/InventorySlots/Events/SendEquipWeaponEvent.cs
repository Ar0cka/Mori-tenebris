using Actors.Player.AttackSystem.Data;
using DefaultNamespace.Enums;

namespace Player.Inventory
{
    public class SendEquipWeaponEvent
    {
        public int WeaponDamage { get; private set; }
        public DamageType DamageType { get; private set; }

        public WeaponAttackSettings WeaponConfig { get; private set; }

        public SendEquipWeaponEvent(int weaponDamage, DamageType damageType, WeaponAttackSettings weaponConfig)
        {
            WeaponDamage = weaponDamage;
            DamageType = damageType;
            WeaponConfig = weaponConfig;
        }
    }
}