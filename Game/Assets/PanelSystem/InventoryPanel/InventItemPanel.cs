using DefaultNamespace;
using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventItemPanel : ItemPanelSystem
{
    [SerializeField] private InventoryPanel inventoryPanel;
    
    [SerializeField] private Button removeButton;
    [SerializeField] private Button useButton;

    [SerializeField] private GameObject removePanel;
    
    public override void Open(ItemUI item)
    {
        base.Open(item);
        
        removeButton.onClick.AddListener(RemoveItem);
        useButton.onClick.AddListener(PanelAction);
    }

    protected override void PanelAction()
    {
        inventoryPanel.ItemRouter(CurrentItem);
        Close();
    }

    private void RemoveItem()
    {
        if (removePanel != null && !removePanel.activeInHierarchy)
        {
            removePanel.SetActive(true);
        }
    }

    public override void Close()
    {
        base.Close();
        removeButton.onClick.RemoveListener(RemoveItem);
        useButton.onClick.RemoveListener(PanelAction);
    }
}
