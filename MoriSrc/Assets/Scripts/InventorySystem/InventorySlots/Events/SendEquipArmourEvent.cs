namespace Player.Inventory
{
    public class SendEquipArmourEvent
    {
        public float PhysicArmour { get; private set; }
        public float MagicArmour { get; private set; }

        public SendEquipArmourEvent(float physicArmour, float magicArmour)
        {
            PhysicArmour = physicArmour;
            MagicArmour = magicArmour;
        }
    }
}