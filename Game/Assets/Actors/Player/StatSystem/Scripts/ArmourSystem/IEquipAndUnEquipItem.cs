using Enemy;

namespace PlayerNameSpace
{
    public interface IEquipAndUnEquipItem
    {
        void EquipArmourItem(ItemData itemData);
        void DeleteArmour(string name);
    }
}