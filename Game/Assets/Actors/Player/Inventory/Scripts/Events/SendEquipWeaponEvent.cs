using DefaultNamespace.Enums;

namespace Player.Inventory
{
    public class SendEquipWeaponEvent
    {
        public int WeaponDamage { get; private set; }
        public DamageType DamageType { get; private set; }

        public SendEquipWeaponEvent(int weaponDamage, DamageType damageType)
        {
            WeaponDamage = weaponDamage;
            DamageType = damageType;
        }
    }
}