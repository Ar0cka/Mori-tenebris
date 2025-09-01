using Player.Inventory;

namespace DefaultNamespace
{
    public interface IPanelOpen
    {
        void Open(ItemUI itemUI);
        void Close();
    }
}