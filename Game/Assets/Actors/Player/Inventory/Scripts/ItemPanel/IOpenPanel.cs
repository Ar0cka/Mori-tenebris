using Player.Inventory;

namespace Actors.Player.Inventory.Scripts.ItemPanel
{
    public interface IOpenPanel
    {
        void OpenPanel(ItemInstance itemInstance, ItemAction itemAction, bool isEquiped);
    }
}