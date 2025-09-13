using DefaultNamespace;
using Player.Inventory;
using UnityEngine;

namespace TestingFolder.Craft
{
    public class OpenCraft : MonoBehaviour
    {
        [SerializeField] private CraftPanel craftPanel;
        [SerializeField] private InventoryPanel inventoryPanel;

        public void Open()
        {
            craftPanel.OpenPanel(new CraftContext(inventoryPanel.GetInventoryLogic()));
        }
    }
}