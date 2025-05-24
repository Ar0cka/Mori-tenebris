using Player.Inventory;

namespace Actors.Player.Inventory.Scripts.ItemPanel
{
    public interface IOpenPanel
    {
        void OpenPanel(ItemScrObj itemScrObj, ItemAction itemAction, bool isEquiped);
    }
}