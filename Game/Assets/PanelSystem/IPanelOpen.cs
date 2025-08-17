using Player.Inventory;

namespace DefaultNamespace
{
    public interface IPanelOpen
    {
        void Open(ItemSettings itemSettings);
        void Close();
    }
}