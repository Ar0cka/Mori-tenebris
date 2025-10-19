using Player.Inventory;

namespace DefaultNamespace
{
    public interface IPanelOpen <TValue>
    {
        void Open(TValue itemUI);
        void Close();
    }
}