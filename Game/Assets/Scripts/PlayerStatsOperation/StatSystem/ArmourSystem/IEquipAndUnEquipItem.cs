namespace DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem
{
    public interface IEquipAndUnEquipItem
    {
        void EquipArmourItem(ItemData itemData);
        void DeleteArmour(string name);
    }
}